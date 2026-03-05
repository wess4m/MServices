using CouponAPI.Data;
using CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        readonly ApplicationDBContext _db;
        private ResponseDto _Resp;
        private IMapper _map;
        public CouponController(ApplicationDBContext db, IMapper map)
        {
            _db = db;
            _Resp = new();
            _map = map;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                _Resp.Result = _map.Map<List<CouponDto>>(_db.Coupons.ToList());
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
        public ResponseDto ResponseDto(int id)
        {
            try
            {
                _Resp.Result = _map.Map<CouponDto>(_db.Coupons.Where(x => x.CouponId == id).First());
            }
            catch (Exception ex)
            {
                _Resp.IsSuccess = false;
                _Resp.Message = ex.Message;
            }
            return _Resp;
        }
        [HttpGet]
        [Route("{Code}")]
        public ResponseDto GetByCode(string Code)
        {
            try
            {
                _Resp.Result = _map.Map<CouponDto>(_db.Coupons.Where(x => x.CouponCode.ToLower() == Code.ToLower().TrimEnd()).First());
            }
            catch (Exception ex)
            {
                _Resp.IsSuccess = false;
                _Resp.Message = ex.Message;
            }
            return _Resp;
        }
        [HttpPost]
        public ResponseDto Add([FromBody] CouponDto dto)
        {
            try
            {
                _db.Coupons.Add(_map.Map<Coupon>(dto));
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
        public ResponseDto Update([FromBody] CouponDto dto)
        {
            try
            {
                _db.Coupons.Update(_map.Map<Coupon>(dto));
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
        public ResponseDto Delete(int Id)
        {
            try
            {
                _db.Coupons.Where(x=>x.CouponId == Id).ExecuteDelete();
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
