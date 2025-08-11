using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.UseCases.Friend.Commands
{
    public class UnBlockFriendCommand : IRequest<APIResponse>{
        public long FriendID { get; set; }
    }

    public class UnBlockFriendCommandHandler : IRequestHandler<UnBlockFriendCommand, APIResponse>
    {
        private readonly IFriendRepository _friendRepository;

        private readonly IIdentityService _identityService;
        
        public UnBlockFriendCommandHandler(IFriendRepository friendRepository, IIdentityService identityService)
        {
            _friendRepository = friendRepository;
            _identityService = identityService;
        }

        public async Task<APIResponse> Handle(UnBlockFriendCommand request, CancellationToken cancellationToken)
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

                var status = await _friendRepository.UnblockAsync(userSession.Data.UserID, request.FriendID, fullname);
                
                return new APIResponse{
                    Code = status,
                    Message = "Friend unblocked",
                };
            }
            catch (Exception ex)
            {
                return new APIResponse{
                    Code = 0,
                    Message = "Unblock friend failed: " + ex.Message,
                };
            }
        }
    }
}
