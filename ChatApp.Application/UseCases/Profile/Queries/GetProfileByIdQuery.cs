using MediatR;
using ChatApp.Application.DTOs;
using ChatApp.Domain.Repositories;
using ChatApp.Application.DTOs.Profile;
using ChatApp.Application.Model;
using FluentValidation;

namespace ChatApp.Application.UseCases.Profile.Queries
{
    public class GetProfileByIdQuery : IRequest<APIResponse<UserProfileDTO>>
    {
        public long UserID { get; set; }
    }

    public class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, APIResponse<UserProfileDTO>>
    {
        private readonly IProfileRepository _profileRepository;

        public GetProfileByIdQueryHandler(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
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
               var profile = await _profileRepository.GetProfileAsync(request.UserID);
               
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