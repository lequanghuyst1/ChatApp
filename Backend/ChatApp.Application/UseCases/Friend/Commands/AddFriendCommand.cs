using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.UseCases.Friend.Commands
{
    public record AddFriendCommand(long FriendID) : IRequest<APIResponse>;

    public class AddFriendCommandHandler : IRequestHandler<AddFriendCommand, APIResponse>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IIdentityService _identityService;

        public AddFriendCommandHandler(IFriendRepository friendRepository, IIdentityService identityService)
        {
            _friendRepository = friendRepository;
            _identityService = identityService;
        }
        
        public async Task<APIResponse> Handle(AddFriendCommand request, CancellationToken cancellationToken)
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

                var result = await _friendRepository.AddFriendRequestAsync(userSession.Data.UserID, request.FriendID);

                return new APIResponse{
                    Code = result.status,
                    Message = "Friend request sent",
                };
            }
            catch (Exception ex)
            {
                return new APIResponse{
                    Code = 0,
                    Message = "Add friend failed: " + ex.Message,
                };
            }
        }
    }
}
