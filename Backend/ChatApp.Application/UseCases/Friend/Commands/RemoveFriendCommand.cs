using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.UseCases.Friend.Commands
{
    public record RemoveFriendCommand(long FriendID) : IRequest<APIResponse>;

    public class RemoveFriendCommandHandler : IRequestHandler<RemoveFriendCommand, APIResponse>
    {
        private readonly IFriendRepository _friendRepository;

        private readonly IIdentityService _identityService;

        public RemoveFriendCommandHandler(IFriendRepository friendRepository, IIdentityService identityService)
        {
            _friendRepository = friendRepository;
            _identityService = identityService;
        }

        public async Task<APIResponse> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userSession = _identityService.GetUser<UserProfile>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse{
                        Code = 0,
                        Message = "User is not authenticated",
                    };
                }

                var fullname = $"{userSession.Data.FirstName} {userSession.Data.LastName}";

                var status = await _friendRepository.DeleteAsync(userSession.Data.UserID, request.FriendID, fullname);

                return new APIResponse{
                    Code = status,
                    Message = "Friend removed",
                };
            }
            catch (Exception ex)
            {
                return new APIResponse{
                    Code = 0,
                    Message = "Remove friend failed: " + ex.Message,
                };
            }
        }
    }
}
