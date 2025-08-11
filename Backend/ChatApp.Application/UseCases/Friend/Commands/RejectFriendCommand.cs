using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.UseCases.Friend.Commands
{
    public record RejectFriendCommand(long friendID) : IRequest<APIResponse>;

    public class RejectFriendCommandHandler : IRequestHandler<RejectFriendCommand, APIResponse>
    {
        private readonly IFriendRepository _friendRepository;

        private readonly IIdentityService _identityService;
        
        public RejectFriendCommandHandler(IFriendRepository friendRepository, IIdentityService identityService)
        {
            _friendRepository = friendRepository;
            _identityService = identityService;
        }

        public async Task<APIResponse> Handle(RejectFriendCommand request, CancellationToken cancellationToken)
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

                var status = await _friendRepository.RejectAsync(userSession.Data.UserID, request.friendID, fullname);
                
                return new APIResponse{
                    Code = status,
                    Message = "Friend request rejected",
                };
            }
            catch (Exception ex)
            {
                return new APIResponse{
                    Code = 0,
                    Message = "Reject friend failed: " + ex.Message,
                };
            }
        }
    }
}
