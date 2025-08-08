using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Repositories;
using ChatApp.Utilities.Util;
using ChatApp.Utitilies.Security;
using FluentValidation;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatApp.Application.UseCases.Account.Commands
{

    public record RegisterResponse
    {
        public UserProfile UserProfile { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public record RegisterUserCommand : IRequest<APIResponse<RegisterResponse>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, APIResponse<RegisterResponse>>
    {

        private readonly IAccountRepository _accountRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly ITokenService _tokenService;

        public RegisterUserCommandHandler(IAccountRepository accountRepository, IProfileRepository profileRepository, ITokenService tokenService)
        {
            _accountRepository = accountRepository;
            _profileRepository = profileRepository;
            _tokenService = tokenService;
        }
        
        public async Task<APIResponse<RegisterResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _accountRepository.GetAccountByUsernameAsync(request.Username);

                
                if (account != null)
                {
                    return new APIResponse<RegisterResponse>
                    {
                        Code = -99,
                        Message = "Tài khoản đã tồn tại"
                    };
                }

                AccountType accountType = AccountType.PHONE;
                if (PolicyUtil.ValidateEmail(request.Username))
                    accountType = AccountType.EMAIL;

                var newID = await _accountRepository.CreateAsync(request.Username, SecurityLib.MD5Encrypt(request.Password), request.FirstName, request.LastName, accountType);

                var userProfile =  await _profileRepository.GetProfileAsync(newID);

                if (userProfile == null)
                {
                    return new APIResponse<RegisterResponse>
                    {
                        Code = -99,
                        Message = "Thông tin không tồn tại"
                    };
                }

                var profile = new UserProfile
                {
                    UserID = userProfile.UserID,
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    Avatar = userProfile.Avatar,
                    DateOfBirth = userProfile.DateOfBirth,
                    Phone = userProfile.Phone,
                    Email = userProfile.Email,
                    Gender = userProfile.Gender,
                    Bio = userProfile.Bio
                };


                var sessionId = Guid.NewGuid();
                var tokens = _tokenService.GenerateToken(new UserSession<UserProfile>
                {
                    UserID = profile.UserID,
                    Data = profile,
                    SessionID = sessionId
                });

                return new APIResponse<RegisterResponse>
                {
                    Code = 1,
                    Data = new RegisterResponse
                    {
                        UserProfile = profile,
                        AccessToken = tokens.AccessToken,
                        RefreshToken = tokens.RefreshToken
                    },
                    Message = "Thành công"
                };

            }
            catch (Exception ex)
            {
                return new APIResponse<RegisterResponse>
                {
                    Code = -99,
                    Message = ex.Message
                };
            }
        }
    }
}