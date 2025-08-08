using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.UseCases.Friend.Queries
{
    public record GetFriendListQuery() : IRequest<APIResponse<IEnumerable<UserFriend>>>;

    public class GetFriendListQueryHandler : IRequestHandler<GetFriendListQuery, APIResponse<IEnumerable<UserFriend>>>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IIdentityService _identityService;

        public GetFriendListQueryHandler(IFriendRepository friendRepository, IIdentityService identityService)
        {
            _friendRepository = friendRepository;
            _identityService = identityService;
        }

        public async Task<APIResponse<IEnumerable<UserFriend>>> Handle(GetFriendListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userSession = _identityService.GetUser<UserSession>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse<IEnumerable<UserFriend>>{
                        Code = 0,
                        Message = "User is not authenticated",
                    };
                }
        
                var result = await _friendRepository.GetListByUserAsync(userSession.Data.UserID);
        
                return new APIResponse<IEnumerable<UserFriend>>{
                    Code = 1,
                    Data = result,
                    Message = "Get friend list success",
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<UserFriend>>{
                    Code = 0,
                    Message = "Get friend list failed: " + ex.Message,
                };
            }
        }
    }
}
