using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Dapper;
using UserNamespace;

namespace webapplication.Controllers
{

    public sealed class PasswordHash
    {
        const int SaltSize = 16, HashSize = 20, HashIter = 1000;
        readonly byte[] _salt, _hash;
        public PasswordHash(string password)
        {
            new RNGCryptoServiceProvider().GetBytes(_salt = new byte[SaltSize]);
            _hash = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
        }
        public PasswordHash(byte[] hashBytes)
        {
            Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
        }
        public PasswordHash(byte[] salt, byte[] hash)
        {
            Array.Copy(salt, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hash, 0, _hash = new byte[HashSize], 0, HashSize);
        }
        public byte[] ToArray()
        {
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);
            return hashBytes;
        }
        public byte[] Salt { get { return (byte[])_salt.Clone(); } }
        public byte[] Hash { get { return (byte[])_hash.Clone(); } }
        public bool Verify(string password)
        {
            byte[] test = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
            for (int i = 0; i < HashSize; i++)
                if (test[i] != _hash[i])
                    return false;
            return true;
        }
    }

    [Route("api/auth")]
    public class AuthController : Controller 
    {   
        string connectionstring;

        public AuthController (IConfiguration configuration)
        {
            this.connectionstring = configuration.GetConnectionString("DefaultConnection");
        }

        // GET api/values
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]UserModel user)
        {

            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            UserModel dbUser = UserModel.getUser(this.connectionstring, user.Email);

            if(dbUser == null)
            {
                return BadRequest("Invalid client request");
            }

            string savedPasswordHash  = Convert.ToString( dbUser.Password );

            //Check HAsh
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            PasswordHash hash = new PasswordHash(hashBytes);
            if(!hash.Verify(user.Password))
            {
                return Unauthorized();
            }

            //AuthLine
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
            };

            var tokeOptions = new JwtSecurityToken(
                issuer: "http://35.160.149.54",
                audience: "http://35.160.149.54",
                //claims: new List<Claim>(),
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return Ok(new { Token = tokenString });

        }

        [HttpPost, Route("register")]
        public IActionResult Register([FromBody]UserModel user)
        {
            
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            //HASH PW
            PasswordHash hash = new PasswordHash(user.Password);
            byte[] hashBytes = hash.ToArray();  
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            //END HASH

            UserModel.registerUser(this.connectionstring, user.Email, user.FirstName, user.LastName, savedPasswordHash);

            //AuthLine
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                //new Claim(ClaimTypes.Role, "Operator")
            };

            var tokeOptions = new JwtSecurityToken(
                issuer: "http://35.160.149.54",
                audience: "http://35.160.149.54",
                //claims: new List<Claim>(),
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return Ok(new { Token = tokenString });

        }
    }


}
