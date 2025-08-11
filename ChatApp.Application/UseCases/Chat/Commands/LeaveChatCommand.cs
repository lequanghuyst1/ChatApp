using MediatR;
using ChatApp.Application.Model;
using ChatApp.Application.Interfaces;
using ChatApp.Domain.Interfaces;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.UseCases.Chat.Commands
{
    public record LeaveChatCommand() : IRequest<APIResponse<int>>
    {
        public long ChatID { get; set; }
    };

    public class LeaveChatCommandHandler : IRequestHandler<LeaveChatCommand, APIResponse<int>>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IIdentityService _identityService;

        public LeaveChatCommandHandler(IChatRepository chatRepository, IIdentityService identityService)
        {
            _chatRepository = chatRepository;
            _identityService = identityService;
        }

        public async Task<APIResponse<int>> Handle(LeaveChatCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userSession = _identityService.GetUser<UserProfile>();
                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse<int>
                    {
                        Code = 0,
                        Message = "User is not authenticated"
                    };
                }

                var result = await _chatRepository.LeaveAsync(request.ChatID, userSession.Data.UserID);

                if (result != 1)
                {
                    return new APIResponse<int>
                    {
                        Code = 0,
                        Message = "Leave chat failed"
                    };
                }
                return new APIResponse<int>
                {
                    Code = 1,
                    Data = result,
                    Message = "Leave chat successfully"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<int>
                {
                    Code = 0,
                    Message = "Leave chat failed: " + ex.Message
                };
            }
        }
    }
}
