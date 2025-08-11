using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.UseCases.Friend.Queries
{
    public record GetListFriendRequestQuery() : IRequest<APIResponse<IEnumerable<FriendDTO>>>;

    public class GetListFriendRequestQueryHandler : IRequestHandler<GetListFriendRequestQuery, APIResponse<IEnumerable<FriendDTO>>>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IIdentityService _identityService;

        public GetListFriendRequestQueryHandler(IFriendRepository friendRepository, IIdentityService identityService)
        {
            _friendRepository = friendRepository;
            _identityService = identityService;
        }

        public async Task<APIResponse<IEnumerable<FriendDTO>>> Handle(GetListFriendRequestQuery request, CancellationToken cancellationToken)
        {
            var userSession = _identityService.GetUser<UserSession>();
            if (userSession == null || userSession.Data == null)
            {
                return new APIResponse<IEnumerable<FriendDTO>>
                {
                    Code = 0,
                    Message = "User is not authenticated",
                };
            }

            var result = await _friendRepository.GetListFriendRequestAsync(userSession.Data.UserID);

            var friendDTOs = result.Select(friend => new FriendDTO{
                ID = friend.ID,
                UserID = friend.UserID,
                FriendID = friend.FriendID,
                Status = friend.Status,
                AddedAt = friend.AddedAt,
            });
            
            return new APIResponse<IEnumerable<FriendDTO>>
            {
                Code = 1,
                Data = friendDTOs,
                Message = "Get friend requests success",
            };
        }
    }
}
