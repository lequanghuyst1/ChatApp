using MediatR;
using ChatApp.Application.DTOs;
using ChatApp.Domain.Repositories;
using ChatApp.Application.DTOs.Profile;
using ChatApp.Application.Model;
using FluentValidation;
using ChatApp.Application.Interfaces;

namespace ChatApp.Application.UseCases.Profile.Queries
{
    public record GetProfileByIdQuery() : IRequest<APIResponse<UserProfileDTO>> { }

    public class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, APIResponse<UserProfileDTO>>
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IIdentityService _identityService;

        public GetProfileByIdQueryHandler(IProfileRepository profileRepository, IIdentityService identityService)
        {
            _profileRepository = profileRepository;
            _identityService = identityService;
        }

        // public class GetProfileByIdQueryValidator : AbstractValidator<GetProfileByIdQuery>
        // {
        //     public GetProfileByIdQueryValidator()
        //     {

        //         RuleFor(x => x.UserID)
        //             .NotEmpty()
        //             .WithErrorCode("-1")
        //             .WithMessage("")
        //             .GreaterThan(1)
        //             .WithMessage("");
        //     }
        // }

        public async Task<APIResponse<UserProfileDTO>> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
        {
           try
           {
               var userSession = _identityService.GetUser<UserProfileDTO>();
               
               var profile = await _profileRepository.GetProfileAsync(userSession.UserID);
               
               if (profile == null)
               {
                    return new APIResponse<UserProfileDTO>
                    {
                        Code = -99,
                        Message = "Thông tin không tồn tại"
                    };
               }

               return APIResponse<UserProfileDTO>.Success(new UserProfileDTO
               {
                   UserID = profile.UserID,
                   FirstName = profile.FirstName,
                   LastName = profile.LastName,
                   Avatar = profile.Avatar,
                   DateOfBirth = profile.DateOfBirth,
                   Phone = profile.Phone,
                   Email = profile.Email,
                   Gender = profile.Gender,
                   Bio = profile.Bio
               }, "Thành công");
           }
           catch (Exception ex)
           {
                return new APIResponse<UserProfileDTO>
                {
                    Code = -99,
                    Message = ex.Message
                };
            }
        }
    }
}