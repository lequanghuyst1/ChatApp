using MediatR;
using ChatApp.Application.DTOs;
using ChatApp.Domain.Repositories;
using ChatApp.Application.DTOs.Profile;
using ChatApp.Application.Model;
using FluentValidation;
using ChatApp.Application.Interfaces;
using AutoMapper;

namespace ChatApp.Application.UseCases.Profile.Queries
{
    public record GetUsersQuery(string? Keyword) : IRequest<APIResponse<IEnumerable<UserProfileDTO>>>;

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, APIResponse<IEnumerable<UserProfileDTO>>>
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IProfileRepository profileRepository, IIdentityService identityService, IMapper mapper)
        {
            _profileRepository = profileRepository;
            _identityService = identityService;
            _mapper = mapper;
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

        public async Task<APIResponse<IEnumerable<UserProfileDTO>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
           try
           {
                var userSession = _identityService.GetUser<UserProfileDTO>();
               
               var result = await _profileRepository.GetUserListAsync(request.Keyword, userSession.Data.UserID);
               var resultDTO = _mapper.Map<IEnumerable<UserProfileDTO>>(result);

               return APIResponse<IEnumerable<UserProfileDTO>>.Success(resultDTO, "Thành công");
           }
           catch (Exception ex)
           {
                return new APIResponse<IEnumerable<UserProfileDTO>>
                {
                    Code = -99,
                    Message = ex.Message
                };
            }
        }
    }
}
              