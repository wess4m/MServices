using CouponAPI.Data;
using CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CouponAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CouponAPI.Utility;

namespace CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponController : ControllerBase
    {
        readonly ApplicationDBContext _db;
        private ResponseDTO _Resp;
        private IMapper _map;
        public CouponController(ApplicationDBContext db, IMapper map)
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
                _Resp.Result = _map.Map<List<CouponDto>>(_db.Coupons.OrderByDescending(x=>x.CouponId).ToList());
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
        [Route("GetByCode/{Code}")]
        public ResponseDTO GetByCode(string Code)
        {
            try
            {
                _Resp.Result = _map.Map<CouponDto>(_db.Coupons.Where(x => x.CouponCode.ToLower() == Code.ToLower().TrimEnd()).FirstOrDefault());
                if (_Resp.Result == null)
                {
                    _Resp.IsSuccess = false;
                    _Resp.Message = "Invalid coupon code!";
                }
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
        public ResponseDTO Add([FromBody] CouponDto dto)
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
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDTO Update([FromBody] CouponDto dto)
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
        [Route("{id:int}")]
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDTO Delete(int Id)
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
