using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBankIdentity.DTO;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models;
using MyBankIdentityService.Models.ServiceModels;
using MyBankIdentityService.Reporitories.Interfaces;
using MyBankIdentityService.Services.Interfaces;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyBankIdentityService.Utils.Constants;

namespace MyBankIdentityService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IActiveDirectoryService _adService;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepo;
        private readonly IRepository<Branch> _branchRepo;
        private readonly IMapper _mapper;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IActiveDirectoryService activeDirectoryService;

        public AccountService(IActiveDirectoryService adService, IMapper mapper,
                             IRepository<User> userRepo, IRepository<Role> roleRepo, IRepository<Branch> branchRepo,
                             ITokenBuilder tokenBuilder, IActiveDirectoryService activeDirectoryService)
        {
            _adService = adService;
            _mapper = mapper;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _branchRepo = branchRepo;
            _tokenBuilder = tokenBuilder;
            this.activeDirectoryService = activeDirectoryService;

        }

        public async Task<BaseServiceResponse> UpdateUsersBranchInformation()
        {
            try
            {
                var result = new List<User>();
                var allUsers = _userRepo.GetAsync().Where(u => u.IsEnabled).ToList();
                foreach (var user in allUsers)
                {
                    try
                    {
                        var adInfo = activeDirectoryService.GetUserFromAD(user.UserPrincipal);
                        if (!string.IsNullOrWhiteSpace(adInfo?.BranchCode))
                        {
                            user.BranchCode = adInfo.BranchCode;
                            await _userRepo.Complete();

                            result.Add(user);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, $"Exception occured during updating of user data. UserPrincipal({user?.UserPrincipal})");
                    }
                }

                return new BaseServiceResponse(200, result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occured during  updating all of users' data with LDAP");
                return new BaseServiceResponse(500, ex.Message);
            }
        }

        public async Task<BaseServiceResponse> Register(RegisterModel registerModel)
        {
            User user = null;

            try
            {
                user = activeDirectoryService.GetUserFromAD(registerModel.Identity);

                if (user == null)
                {
                    return new BaseServiceResponse(400, ErrorMessage.UserNotFountAD);
                }
                if (user.IsEnabled == false)
                {
                    return new BaseServiceResponse(400, ErrorMessage.UserNotActiveAD);
                }

                var userFromDb = _userRepo.GetAsync().FirstOrDefault(u => u.UserPrincipal == registerModel.Identity || u.HrCode == user.HrCode);

                // User exist in DB but disabled
                if (userFromDb != null && userFromDb.IsEnabled == true)
                {
                    Log.Information($"This user({registerModel.Identity}) already exist in mybank and is active");
                    return new BaseServiceResponse(400, ErrorMessage.UserAlreadyExist);
                }

                if (userFromDb != null)
                {
                    Log.Information($"This user({registerModel.Identity}) already exist in mybank and need to be updated");
                    userFromDb.RoleId = registerModel.RoleId;
                    userFromDb.RefreshToken = null;
                    userFromDb.IsEnabled = user.IsEnabled;

                    var updatedUser = await _userRepo.UpdateAsync(userFromDb);
                    return new BaseServiceResponse(200, updatedUser);
                }

                Log.Information($"This user({registerModel.Identity}) is new for mybnak");
                user.RoleId = registerModel.RoleId;
                user.RefreshToken = null;
                var result = await _userRepo.AddAsync(user);
                return new BaseServiceResponse(200, result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return new BaseServiceResponse(400, ex.Message);
            }
        }

        public async Task<BaseServiceResponse> Login(AuthenticateRequest model)
        {
            AuthenticateResponse authenticateResponse = new AuthenticateResponse();
            try
            {
                Log.Information($"ValidateCredentials - username: {model.Identity}");

                var isValid = activeDirectoryService.ValidateCredentials(model.Identity, model.Password);

                if (!isValid)
                {
                    return new BaseServiceResponse(400, "Username or password is not correct");
                }

                var user = _userRepo.GetAsync().FirstOrDefault(u => u.UserPrincipal == model.Identity && u.IsEnabled == true);

                if (user == null)
                {
                    return new BaseServiceResponse(400, "User not registered or not active");
                }
         
                var userDTO = GetUserDTOAsync(user);
                var token = _tokenBuilder.BuildToken(userDTO);

                authenticateResponse.JwtToken = token;

                var refreshToken = await GetNewRefreshTokenAsync(user.Id);
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return new BaseServiceResponse
                    {
                        Data = "Refresh token can not created",
                        StatusCode = 400
                    };
                }
                authenticateResponse.RefreshToken = refreshToken;


                return new BaseServiceResponse
                {
                    Data = authenticateResponse,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return new BaseServiceResponse
                {
                    Data = $"Error occured. {ex.Message}",
                    StatusCode = 400
                };
            }
        }

        public async Task<BaseServiceResponse> Logout(int id)
        {
            var result = await _tokenBuilder.DeleteByUserIdAsync(id);
            return result;
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            throw new NotImplementedException();
        }


        public async Task<BaseServiceResponse> RefreshToken(string refreshToken)
        {
            AuthenticateResponse response = new AuthenticateResponse();

            if (string.IsNullOrEmpty(refreshToken))
            {
                return new BaseServiceResponse(400, "Token can not be null");
            }

            var refreshTokenFromDB = await _tokenBuilder.GetByRefreshTokenAsync(refreshToken);
            if (refreshTokenFromDB == null)
            {
                return new BaseServiceResponse(404, "Token not exist");
            }

            if (refreshTokenFromDB.IsExpired)
            {
                // delete refresh token
                var isDeleted = await _tokenBuilder.DeleteAsync(refreshTokenFromDB.Id);

                return new BaseServiceResponse(401, "Refresh Token is expired");
            }

            var userDTO = GetUserDTOAsync(refreshTokenFromDB.User);


            var token = _tokenBuilder.BuildToken(userDTO);

            response.JwtToken = token;
            var updatedRefreshToken = await UpdateRefreshTokenAsync(refreshTokenFromDB);
            response.RefreshToken = updatedRefreshToken.Token;

            return new BaseServiceResponse
            {
                Data = response,
                StatusCode = 200
            };

        }


        public string TestTokenGenerator()
        {
            return "test";
            //return _tokenBuilder.BuildToken("satdar.amrahov.a@accessbank.az");
        }


        private async Task<string> GetNewRefreshTokenAsync(int userId)
        {
            var refreshTokenFromDB = await _tokenBuilder.GetByUserIdAsync(userId);
            if (refreshTokenFromDB != null)
            {
                await _tokenBuilder.DeleteAsync(refreshTokenFromDB.Id);
            }
            var refreshToken = await _tokenBuilder.BuildRefreshToken();
            refreshToken.UserId = userId;
            var response = await _tokenBuilder.AddAsync(refreshToken);
            if (response.StatusCode != 200)
            {
                return null;
            }
            refreshToken = response.Data as RefreshToken;
            return refreshToken.Token;
        }

        private async Task<RefreshToken> UpdateRefreshTokenAsync(RefreshToken token)
        {
            var newToken = await _tokenBuilder.BuildRefreshToken();

            token.Expires = newToken.Expires;
            token.Token = newToken.Token;
            var response = await _tokenBuilder.UpdateAsync(token);
            if (response.StatusCode != 200)
            {
                return null;
            }
            token = response.Data as RefreshToken;
            return token;
        }

        private UserDTO GetUserDTOAsync(User user)
        {
            var role = _roleRepo.GetAsync()
                            .Include(a => a.RoleModulesToPermissions)
                                .ThenInclude(a => a.Module)
                            .Include(a => a.RoleModulesToPermissions)
                                    .ThenInclude(a => a.Permission)
                    .FirstOrDefault(r => r.Id == user.RoleId);
            var branchName = _branchRepo.GetAsync().FirstOrDefault(b => b.Code == user.BranchCode)?.Name;

            user.Role = role;
            //user.BranchName = branchName;
            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.BranchName = branchName;
            return userDTO;
        }

        public async Task<BaseServiceResponse> EditRole(UserRoleModel model)
        {
            var user = _userRepo.GetAsync().FirstOrDefault(a => a.Id == model.UserId);
            if (user == null)
            {
                return new BaseServiceResponse(404, "User not found");
            }

            var role = _roleRepo.GetAsync().FirstOrDefault(a => a.Id == model.RoleId);
            if (role == null)
            {
                return new BaseServiceResponse(404, "Role not found");
            }

            user.RoleId = model.RoleId;

            try
            {
                await _userRepo.UpdateAsync(user);
                return new BaseServiceResponse(204);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return new BaseServiceResponse(400, ex.Message);
            }


        }

        public async Task<BaseServiceResponse> EditBranch(UserBranchModel model)
        {
            Log.Information($"Accepted model to update branch of user: {JsonConvert.SerializeObject(model)}");
            var user = _userRepo.GetAsync().FirstOrDefault(a => a.HrCode.ToUpper() == model.HrCode.ToUpper());
            if (user == null)
            {
                Log.Error($"User could not be found with the hrcode: {model.HrCode}");
                return new BaseServiceResponse(200, "User not found");
            }

            var branch = _branchRepo.GetAsync().FirstOrDefault(a => a.Code.ToUpper() == model.BranchCode.ToUpper());
            if (branch == null)
            {
                Log.Error($"Branch could not be found with the branchCode: {model.BranchCode}");
                return new BaseServiceResponse(404, "Branch not found");
            }

            user.BranchCode = branch.Code;

            try
            {
                await _userRepo.UpdateAsync(user);
                Log.Information("User branch is updated");
                return new BaseServiceResponse(204);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occured during updating of users branch");
                return new BaseServiceResponse(400, ex.Message);
            }
        }


        //public async Task<BaseServiceResponse> Users()
        //{
        //    try
        //    {
        //        var users = _userRepo.GetAsync().Where(u => u.IsEnabled == true).Select(u => new {
        //            Id = u.Id,
        //            Email = u.Email,
        //            Fullname = u.Fullname,
        //            HrCode = u.HrCode,
        //            RoleId = u.RoleId
        //        }).ToList();

        //        return new BaseServiceResponse(200, users);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.ToString());
        //        return new BaseServiceResponse(400, ex.Message);
        //    }

        //}

        public async Task<BaseServiceResponse> Users(UsersQueryParam query = null)
        {
            try
            {
                var usersQuery = _userRepo.GetAsync();

                if (query != null)
                {
                    usersQuery = usersQuery.Where(u =>
                              (string.IsNullOrEmpty(query.UserPrincipalName) ? true : u.Email == query.UserPrincipalName) &&
                              (string.IsNullOrEmpty(query.HrCode) ? true : u.HrCode == query.HrCode));
                }

                var users = usersQuery
                    .Include(a => a.Role)
                    .Select(u => new
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Username = u.Username,
                        UserPrincipal = u.UserPrincipal,
                        Fullname = u.Fullname,
                        HrCode = u.HrCode,
                        RoleId = u.RoleId,
                        RoleName = u.Role.Name,
                        IsActive = u.IsEnabled,
                        Branch = _branchRepo.GetAsync().FirstOrDefault(b => b.Code == u.BranchCode)
                    })
                    .ToList();

                return new BaseServiceResponse(200, users);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return new BaseServiceResponse(400, ex.Message);
            }
        }

        public async Task<BaseServiceResponse> ChangeActivation(UserStatusModel model)
        {
            try
            {
                var userFromDB = _userRepo.GetAsync().FirstOrDefault(u => u.Id == model.Id);
                if (userFromDB is null)
                {
                    return new BaseServiceResponse(400, ErrorMessage.UserNotFountDB);
                }
                userFromDB.IsEnabled = model.Status;
                await _userRepo.UpdateAsync(userFromDB);
                return new BaseServiceResponse(200, true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return new BaseServiceResponse(400, ex.Message);
            }
        }
    }
}
