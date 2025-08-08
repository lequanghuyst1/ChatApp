using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.UseCases.Friend.Commands
{
    public record AcceptFriendCommand(long FriendID) : IRequest<APIResponse>;

    public class AcceptFriendCommandHandler : IRequestHandler<AcceptFriendCommand, APIResponse>
    {
        private readonly IFriendRepository _friendRepository;

        private readonly IIdentityService _identityService;
        
        public AcceptFriendCommandHandler(IFriendRepository friendRepository, IIdentityService identityService)
        {
            _friendRepository = friendRepository;
            _identityService = identityService;
        }

        public async Task<APIResponse> Handle(AcceptFriendCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userSession = _identityService.GetUser<UserSession>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse{
                        Code = 0,
                        Message = "User is not authenticated",
                    };
                }

                var status = await _friendRepository.UpdateFriendStatusAsync(userSession.Data.UserID, request.FriendID, FriendStatus.ACCEPTED);
                
                return new APIResponse{
                    Code = status,
                    Message = "Friend request accepted",
                };
            }
            catch (Exception ex)
            {
                return new APIResponse{
                    Code = 0,
                    Message = "Accept friend failed: " + ex.Message,
                };
            }
        }
    }
}
