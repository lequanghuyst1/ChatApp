using MediatR;
using ChatApp.Application.DTOs;
using ChatApp.Application.Model;
using FluentValidation;
using ChatApp.Domain.Repositories;
using ChatApp.Utitilies.Security;
using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.UseCases.Account.Commands
{

    public record LoginResponse
    {
       public UserProfile UserProfile { get; set; }
       public string AccessToken { get; set; }
       public string RefreshToken { get; set; }
    }

    public class LoginUserCommand : IRequest<APIResponse<LoginResponse>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
   
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, APIResponse<LoginResponse>>
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(IProfileRepository profileRepository, IAccountRepository accountRepository, ITokenService tokenService)
        {
            _profileRepository = profileRepository;
            _accountRepository = accountRepository;
            _tokenService = tokenService;
        }

        // public class GetProfileByIdQueryValidator : AbstractValidator<GetProfileByIdQuery>
        // {
        //     public GetProfileByIdQueryValidator()
        //     {

        //         RuleFor(x => x.UserID)
        //             .NotEmpty()  
        //             .WithMessage("User ID is required");
        //     }
        // }
        
        public async Task<APIResponse<LoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.AuthenticateAsync(request.Username, SecurityLib.MD5Encrypt(request.Password));

            if (account == null)
            {
                return new APIResponse<LoginResponse>
                {
                    Code = -99,
                    Message = "Thông tin không tồn tại"
                };
            }

            var userProfile = await _profileRepository.GetProfileAsync(account.UserID);

            if (userProfile == null)
            {
                return new APIResponse<LoginResponse>
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

            return new APIResponse<LoginResponse>
            {
                Code = 1,
                Data = new LoginResponse
                {
                    UserProfile = profile,
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken
                },
                Message = "Thành công"
            };
        }
    }
    
 
}