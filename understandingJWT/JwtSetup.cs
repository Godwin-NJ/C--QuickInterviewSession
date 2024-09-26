using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
//using System.IdentityModel.Tokens.Jwt;

namespace understandingJWT
{
    public class JwtSetup
    {
        IConfiguration config;
        public JwtSetup(IConfiguration _config)
        {
            config = _config;
        }

        public string createJWTToken()
        {
            //JWT security Token
            List<Claim> jwtClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "username" ),
                new Claim(ClaimTypes.Sid, "010101"),
                new Claim(ClaimTypes.Role,"admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Appsettings:Token").Value));

            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var Token = new JwtSecurityToken(
                claims: jwtClaim,
                signingCredentials:credential,
                expires:DateTime.Now.AddHours(2),
                issuer:"Godwin Amadi"
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(Token); //serialized token
          
            return jwtToken;
        }

        public void hashPassword(string password, out byte[] PasswordHash, out byte[] passwordSalt)
        {
            using var hmacPwd = new HMACSHA256();
            passwordSalt = hmacPwd.Key;
            PasswordHash = hmacPwd.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public bool veryHashPassword(byte[] passwordSalt, byte[] passwordHash, string password)
        {
            using (var hmacPwd = new HMACSHA256(passwordSalt))
            {
                var computeHash = hmacPwd.ComputeHash(Encoding.UTF8.GetBytes(password));
                bool ifComputeIsSame = computeHash.Equals(passwordHash);
                return ifComputeIsSame;
            };           
        }

    }
}
