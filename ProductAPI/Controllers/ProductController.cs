using ProductAPI.Data;
using ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ProductAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProductAPI.Utility;

namespace ProductAPI.Controllers
{
    [Route("api/Product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        readonly ApplicationDBContext _db;
        private ResponseDTO _Resp;
        private IMapper _map;
        public ProductController(ApplicationDBContext db, IMapper map)
        {
            _db = db;
            _Resp = new();
            _map = map;
        }

        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                _Resp.Result = _map.Map<List<ProductDto>>(_db.Products.OrderByDescending(x=>x.ProductId).ToList());
            }
            catch (Exception ex)
            {
                _Resp.IsSuccess = false;
                _Resp.Message = ex.Message;
            }
            return _Resp;
        }
        [HttpGet]
        [Route("{id:int}")]
        public ResponseDTO ResponseDTO(int id)
        {
            try
            {
                _Resp.Result = _map.Map<ProductDto>(_db.Products.Where(x => x.ProductId == id).First());
            }
            catch (Exception ex)
            {
                _Resp.IsSuccess = false;
                _Resp.Message = ex.Message;
            }
            return _Resp;
        }
        [HttpPost]
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDTO Add([FromBody] ProductDto dto)
        {
            try
            {
                _db.Products.Add(_map.Map<Product>(dto));
                _db.SaveChanges();
                _Resp.Result = dto;
            }
            catch (Exception ex)
            {
                _Resp.IsSuccess = false;
                _Resp.Message = ex.Message;
            }
            return _Resp;
        }
        [HttpPut]
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDTO Update([FromBody] ProductDto dto)
        {
            try
            {
                _db.Products.Update(_map.Map<Product>(dto));
                _db.SaveChanges();
                _Resp.Result = dto;
            }
            catch (Exception ex)
            {
                _Resp.IsSuccess = false;
                _Resp.Message = ex.Message;
            }
            return _Resp;
        }
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDTO Delete(int Id)
        {
            try
            {
                _db.Products.Where(x=>x.ProductId == Id).ExecuteDelete();
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _Resp.IsSuccess = false;
                _Resp.Message = ex.Message;
            }
            return _Resp;
        }

    }
}
