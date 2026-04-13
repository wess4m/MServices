using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Models.Dto;
using ShoppingCartAPI.Service.IService;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartAPIController : ControllerBase
    {
        private ResponseDTO _responseDTO;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _dbContext;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        public CartAPIController(ApplicationDBContext dbContext, IMapper mapper,
            IProductService productService, ICouponService couponService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _responseDTO = new();
            _productService = productService;
            _couponService = couponService;
        }
        [HttpGet("GetCart/{UserID}")]
        public async Task<ResponseDTO> GetCart(string UserID)
        {
            try
            {
                var cartDto = new CartDto();
                var cartHeader = await _dbContext.CartHeaders.OrderDescending().FirstOrDefaultAsync(x => x.UserId == UserID);
                if (cartHeader != null)
                {
                    var cartDetails = await _dbContext.CartDetails.Where(x => x.CartHeaderId == cartHeader.Id).ToListAsync();
                    IEnumerable<ProductDto> products = await _productService.GetProducts();
                    foreach (var item in cartDetails)
                    {
                        item.Product = products.FirstOrDefault(x => x.ProductId == item.ProductId);
                    }
                    // Calculate the cart total after applying coupon discount
                    if (cartHeader.CouponCode != null)
                    {
                        cartHeader.CartTotal = cartHeader.CartTotal - (cartHeader.CartTotal * cartHeader.Discount / 100);
                    }
                    cartDto.CartDetailsDto = _mapper.Map<List<CartDetailsDto>>(cartDetails);
                    cartDto.CartHeaderDto = _mapper.Map<CartHeaderDto>(cartHeader);
                }
                _responseDTO.Result = cartDto;
                return _responseDTO;
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;
                return _responseDTO;
            }
        }
        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO> CartUpsert(CartDto cartDTO)
        {
            try
            {
                var cartHeader = await _dbContext.CartHeaders.OrderDescending().FirstOrDefaultAsync(x => x.UserId == cartDTO.CartHeaderDto.UserId);
                if (cartHeader == null)
                {
                    // Create new header and details
                    cartHeader = _mapper.Map<CartHeader>(cartDTO.CartHeaderDto);
                    _dbContext.CartHeaders.Add(cartHeader);
                    await _dbContext.SaveChangesAsync();
                    cartDTO.CartDetailsDto.First().CartHeaderId = cartHeader.Id;
                    _dbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetailsDto.First()));
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    var cartDetails = await _dbContext.CartDetails.FirstOrDefaultAsync(
                        x => x.ProductId == cartDTO.CartDetailsDto.First().ProductId && x.CartHeaderId == cartDTO.CartHeaderDto.Id);
                    if (cartDetails == null)
                    {
                        // Create new details
                        cartDTO.CartDetailsDto.First().CartHeaderId = cartHeader.Id;
                        _dbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetailsDto.First()));
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        // Update current details product count
                        cartDetails.Count += cartDTO.CartDetailsDto.First().Count;
                        _dbContext.CartDetails.Update(cartDetails);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                //Here we will get products from product api to calculate total price of the cart
                IEnumerable<ProductDto> products = await _productService.GetProducts();
                var cartDetailsLst = await _dbContext.CartDetails.Where(x => x.CartHeaderId == cartHeader.Id).ToListAsync();
                double cartTotal = 0;
                foreach (var item in cartDetailsLst)
                {
                    item.Product = products.FirstOrDefault(x => x.ProductId == item.ProductId);
                    cartTotal += item.Count * item.Product.Price;
                }
                cartHeader.CartTotal = cartTotal;
                _dbContext.CartHeaders.Update(cartHeader);
                await _dbContext.SaveChangesAsync();

                _responseDTO.Result = cartDTO;
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;
            }
            return _responseDTO;
        }
        [HttpPost("RemoveCartItem/{cartDetailsId}")]
        public async Task<ResponseDTO> RemoveCartItem(int cartDetailsId)
        {
            try
            {
                var cDetails = await _dbContext.CartDetails.FirstOrDefaultAsync(x => x.Id == cartDetailsId);
                int cartHeaderID = cDetails.CartHeaderId;
                _dbContext.CartDetails.Remove(cDetails);
                await _dbContext.SaveChangesAsync();
                int TotalCount = _dbContext.CartDetails.Where(x => x.CartHeaderId == cartHeaderID).Sum(x => x.Count);
                if (TotalCount == 0)
                {
                    await _dbContext.CartHeaders.Where(x => x.Id == cartHeaderID).ExecuteDeleteAsync();
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    IEnumerable<ProductDto> products = await _productService.GetProducts();
                    var cartDetailsLst = await _dbContext.CartDetails.Where(x => x.CartHeaderId == cartHeaderID).ToListAsync();
                    double cartTotal = 0;
                    foreach (var item in cartDetailsLst)
                    {
                        item.Product = products.FirstOrDefault(x => x.ProductId == item.ProductId);
                        cartTotal += item.Count * item.Product.Price;
                    }
                    var cartHeader = _dbContext.CartHeaders.Where(x => x.Id == cartHeaderID).FirstOrDefault();
                    cartHeader.CartTotal = cartTotal;
                    _dbContext.CartHeaders.Update(cartHeader);
                    await _dbContext.SaveChangesAsync();
                    _responseDTO.Result = cartDetailsLst.Sum(x => x.Count);
                }
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;
            }
            return _responseDTO;
        }
        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDTO> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var coupon = await _couponService.GetCouponByCode(cartDto.CartHeaderDto.CouponCode);
                if (coupon == null)
                {
                    _responseDTO.IsSuccess = false;
                    _responseDTO.Message = "Invalid coupon code!";
                    return _responseDTO;
                }
                var HeaderFromDb = await _dbContext.CartHeaders.FirstOrDefaultAsync(x => x.Id == cartDto.CartHeaderDto.Id);
                if (HeaderFromDb != null && HeaderFromDb.CartTotal < coupon.MinAmount)
                {
                    _responseDTO.IsSuccess = false;
                    _responseDTO.Message = $"The cart total amount is less than the min amount of {coupon.MinAmount.ToString("c")}";
                    return _responseDTO;
                }
                HeaderFromDb.CouponCode = coupon.CouponCode;
                HeaderFromDb.Discount = coupon.DiscountAmount;
                _dbContext.CartHeaders.Update(HeaderFromDb);
                await _dbContext.SaveChangesAsync();
                _responseDTO.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;
            }
            return _responseDTO;
        }
        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDTO> RemoveCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var HeaderFromDb = await _dbContext.CartHeaders.FirstOrDefaultAsync(x => x.Id == cartDto.CartHeaderDto.Id);
                HeaderFromDb.CouponCode = null;
                HeaderFromDb.Discount = 0;
                _dbContext.CartHeaders.Update(HeaderFromDb);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = e.Message;
            }
            return _responseDTO;
        }
    }
            
}
