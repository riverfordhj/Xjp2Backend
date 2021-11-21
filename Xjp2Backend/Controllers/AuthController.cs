using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Models;
using Models.Authentication.JWT;
using Models.Authentication.JWT.AuthHelper;
using Models.DataHelper;

namespace Xjp2Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;

        private XjpRepository _repository = null;
        private readonly StreetContext _context;
        public AuthController(StreetContext xjpContext, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, IUserService userService, IMemoryCache cache)
        {
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _userService = userService;
            _cache = cache;

            _context = xjpContext;
            _repository = new XjpRepository(xjpContext);
        }

        /// <summary>
        /// Log in
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var user = _repository.GetUserByName(request.UserName);

            if (user == null)
            {
                ModelState.AddModelError("login_failure", "Invalid username.");
                return BadRequest(ModelState);
            }
            if (!passwordHasher.VerifyHashedPassword(request.Password, user.Password))
            {
                ModelState.AddModelError("login_failure", "Invalid password.");
                return BadRequest(ModelState);
            }
            //if (!request.Password.Equals(user.Password))
            //{
            //    ModelState.AddModelError("login_failure", "Invalid password.");
            //    return BadRequest(ModelState);
            //}

            string refreshToken = Guid.NewGuid().ToString();
            var claimsIdentity = _jwtFactory.GenerateClaimsIdentity(user);

            _cache.Set(refreshToken, user.UserName, TimeSpan.FromMinutes(11));

            var token = await _jwtFactory.GenerateEncodedToken(user.UserName, refreshToken, claimsIdentity);
            return new OkObjectResult(token);
        }

        /// <summary>
        /// RefreshToken
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            string userName;
            if (!_cache.TryGetValue(request.RefreshToken, out userName))
            {
                ModelState.AddModelError("refreshtoken_failure", "Invalid refreshtoken.");
                return BadRequest(ModelState);
            }
            if (!request.UserName.Equals(userName))
            {
                ModelState.AddModelError("refreshtoken_failure", "Invalid userName.");
                return BadRequest(ModelState);
            }

            //var user = _userService.GetUserByName(request.UserName);
            var user = _repository.GetUserByName(request.UserName);
            string newRefreshToken = Guid.NewGuid().ToString();
            var claimsIdentity = _jwtFactory.GenerateClaimsIdentity(user);

            _cache.Remove(request.RefreshToken);
            _cache.Set(newRefreshToken, user.UserName, TimeSpan.FromMinutes(11));

            var token = await _jwtFactory.GenerateEncodedToken(user.UserName, newRefreshToken, claimsIdentity);
            return new OkObjectResult(token);
        }

        /// <summary>
        /// Get User Info
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Name;
            return Ok(claimsIdentity.Claims.ToList().Select(r=> new { r.Type, r.Value}));
        }

        //更新用户密码
        [HttpPost("[action]")]
        public object UpdatePassword([FromBody] UpdatePasswordParam FormObj)
        {
            var res = _context.Users.SingleOrDefault(u => u.UserName == FormObj.Account);
            if(res == null)
            {
                return new
                {
                    message = "该用户不存在"
                };
            }
            else if(!passwordHasher.VerifyHashedPassword(FormObj.LastPassword, res.Password))
            {
                return new
                {
                    message = "密码错误"
                };
            }
            else
            {
                res.Password = passwordHasher.HashPassword(FormObj.Password);
                _context.SaveChanges();
                return new
                {
                    message = "密码修改成功"
                };
            }
           
        }
        //返回所有用户信息
        [HttpGet("[action]")]
        [Authorize(Roles = "Administrator")]
        public IEnumerable<object> GetUsersData()
        {
            return from u in _context.Users.Where(u => true)
                   select new
                   {
                       u.Id,
                       u.UserName,
                       u.Password,
                       u.phone
                   };
                   
        }

        [HttpPost("[action]")]
        public IEnumerable<object> ResetPassword(List<ResetUserPasswordParam> FormList)
        {
            foreach(var item in FormList)
            {
                var res = _context.Users.SingleOrDefault(u => u.UserName == item.UserName);
                if(res != null)
                {
                    res.Password = passwordHasher.HashPassword("123456");
                    _context.SaveChanges();
                }
            }

            return GetUsersData();
        }
    }
}