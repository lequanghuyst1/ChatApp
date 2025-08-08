using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.UseCases.Friend.Queries
{
    public record GetListFriendRequestQuery() : IRequest<APIResponse<IEnumerable<UserFriend>>>;

    public class GetListFriendRequestQueryHandler : IRequestHandler<GetListFriendRequestQuery, APIResponse<IEnumerable<UserFriend>>>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IIdentityService _identityService;

        public GetListFriendRequestQueryHandler(IFriendRepository friendRepository, IIdentityService identityService)
        {
            _friendRepository = friendRepository;
            _identityService = identityService;
        }

        public async Task<APIResponse<IEnumerable<UserFriend>>> Handle(GetListFriendRequestQuery request, CancellationToken cancellationToken)
        {
            var userSession = _identityService.GetUser<UserSession>();
            if (userSession == null || userSession.Data == null)
            {
                return new APIResponse<IEnumerable<UserFriend>>
                {
                    Code = 0,
                    Message = "User is not authenticated",
                };
            }

            var result = await _friendRepository.GetListFriendRequestAsync(userSession.Data.UserID);
            
            return new APIResponse<IEnumerable<UserFriend>>
            {
                Code = 1,
                Data = result,
                Message = "Get friend requests success",
            };
        }
    }
}
