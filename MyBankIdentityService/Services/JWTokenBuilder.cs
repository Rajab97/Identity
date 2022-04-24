using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBankIdentity.DTO;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models.ConfigModels;
using MyBankIdentityService.Models.ServiceModels;
using MyBankIdentityService.Reporitories.Interfaces;
using MyBankIdentityService.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services
{
    public class JWTokenBuilder : ITokenBuilder
    {
        private readonly JWTConfig _jwtConfig;
        protected readonly IRepository<RefreshToken> _refreshRepo;

        public JWTokenBuilder(JWTConfig jwtConfig, IRepository<RefreshToken> refreshRepo)
        {
            _jwtConfig = jwtConfig;
            _refreshRepo = refreshRepo;
        }

        public async Task<RefreshToken> BuildRefreshToken()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow
                };
            }
        }

        public string BuildToken(UserDTO userDTO)
        {
            var claimsDict = new Dictionary<string, object>();
            claimsDict.Add("user", userDTO);
            claimsDict.Add("id", userDTO.Email);


            var nbf = DateTime.UtcNow.AddSeconds(-1);
            var exp = DateTime.UtcNow.AddSeconds(_jwtConfig.ExpireWithSeconds);
            var payload = new JwtPayload("", "", new List<Claim>(), notBefore: nbf, expires: exp)
            {
                { "user", userDTO }
            };


            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var header = new JwtHeader(credentials);

            var secToken = new JwtSecurityToken(header, payload);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(secToken);
            return token;

            //return encodedJwt;
        }

        public async Task<BaseServiceResponse> AddAsync(RefreshToken value)
        {
            try
            {
                var repoResponse = await _refreshRepo.AddAsync(value);
                return new BaseServiceResponse(200, repoResponse);
            }
            catch (Exception ex)
            {
                return new BaseServiceResponse(400, ex.Message);
            }
        }


        public async Task<BaseServiceResponse> DeleteAsync(int id)
        {
            try
            {
                var entity = _refreshRepo.GetAsync().FirstOrDefault(a => a.Id == id);
                var repoResponse = await _refreshRepo.DeleteAsync(entity);
                return new BaseServiceResponse(200, repoResponse);
            }
            catch (ArgumentNullException ex)
            {
                return new BaseServiceResponse(404, ex.Message);
            }
            catch (Exception ex)
            {
                return new BaseServiceResponse(400, ex.Message);
            }
        }

        public async Task<BaseServiceResponse> DeleteByUserIdAsync(int id)
        {
            try
            {
                var entity = _refreshRepo.GetAsync().FirstOrDefault(a => a.UserId == id);
                var repoResponse = await _refreshRepo.DeleteAsync(entity);
                return new BaseServiceResponse(200, repoResponse);
            }
            catch (ArgumentNullException ex)
            {
                return new BaseServiceResponse(404, ex.Message);
            }
            catch (Exception ex)
            {
                return new BaseServiceResponse(400, ex.Message);
            }
        }

        public async Task<RefreshToken> GetByRefreshTokenAsync(string token)
        {
            try
            {
                var repoResponse = _refreshRepo.GetAsync()
                    .Include(a => a.User)
                    .Where(a=>a.User.IsEnabled)
                    .FirstOrDefault(x => x.Token == token);
                return repoResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return null;
            }
        }
      
        public async Task<RefreshToken> GetByUserIdAsync(int id)
        {
            try
            {
                var repoResponse = await _refreshRepo.GetAsync()
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(x => x.UserId == id);
                return repoResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return null;
            }
        }

        public async Task<BaseServiceResponse> UpdateAsync(RefreshToken value)
        {
            try
            {
                var repoResponse = await _refreshRepo.UpdateAsync(value);
                return new BaseServiceResponse(200, repoResponse);
            }
            catch (ArgumentNullException ex)
            {
                return new BaseServiceResponse(404, ex.Message);
            }
            catch (Exception ex)
            {
                return new BaseServiceResponse(400, ex.Message);
            }
        }
    }
}
