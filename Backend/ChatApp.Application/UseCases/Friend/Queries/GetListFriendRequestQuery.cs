using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Interfaces;
using MediatR;
using AutoMapper;

namespace ChatApp.Application.UseCases.Friend.Queries
{
    public record GetListFriendRequestQuery() : IRequest<APIResponse<IEnumerable<FriendDTO>>>;

    public class GetListFriendRequestQueryHandler : IRequestHandler<GetListFriendRequestQuery, APIResponse<IEnumerable<FriendDTO>>>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetListFriendRequestQueryHandler(IFriendRepository friendRepository, IIdentityService identityService, IMapper mapper)
        {
            _friendRepository = friendRepository;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<APIResponse<IEnumerable<FriendDTO>>> Handle(GetListFriendRequestQuery request, CancellationToken cancellationToken)
        {
            var userSession = _identityService.GetUser<UserSession>();
            if (userSession == null || userSession.Data == null)
            {
                return new APIResponse<IEnumerable<FriendDTO>>
                {
                    Code = -1,
                    Message = "User is not authenticated",
                };
            }

            var result = await _friendRepository.GetListFriendRequestAsync(userSession.Data.UserID);

            var friendDTOs = _mapper.Map<IEnumerable<FriendDTO>>(result);
            
            return new APIResponse<IEnumerable<FriendDTO>>
            {
                Code = 1,
                Data = friendDTOs,
                Message = "Get friend requests success",
            };
        }
    }
}
