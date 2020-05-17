using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FutsalSystem.Models;
using FutsalSystem.Models.DTO.User;
using FutsalSystem.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FutsalSystem.Controllers
{
    [Route("api")]
    public class JwtAuthController : Controller
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        public JwtAuthController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GenerateToken([FromBody] UserDTO userDTO)
        {
            var users = await _repository.QueryAsync<User>();
            var user = _mapper.Map<User>(userDTO);
            bool exists = false;
            int tempId = 0;
            foreach (var element in users)
            {
                if (element.Username == user.Username && element.Password == user.Password)
                {
                    exists = true;
                    tempId = element.Id;
                }
            }
            if (exists)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsSecurityKey954785!@!!!"));
                var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                var token = new JwtSecurityToken(
                    issuer: "mysite.com",
                    audience: "mysite.com",
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: signInCred
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                var sentToken = new { tokenString, tempId };
                return Ok(sentToken);
            }

            return Unauthorized();
        }
    }
}
