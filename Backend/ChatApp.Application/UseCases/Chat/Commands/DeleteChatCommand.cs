using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using MediatR;
using System.Transactions;

namespace ChatApp.Application.UseCases.Chat.Commands
{
    public class DeleteChatCommand : IRequest<APIResponse>
    {
       public long ChatID { get; set; }
    }

    public class DeleteChatCommandHandler : IRequestHandler<DeleteChatCommand, APIResponse>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IIdentityService _identityService;
        
        public DeleteChatCommandHandler(IChatRepository chatRepository, IIdentityService identityService)
        {
            _chatRepository = chatRepository;
            _identityService = identityService;
        }
        
        public async Task<APIResponse> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
        {
            try{
                var userSession = _identityService.GetUser<UserProfile>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse {
                        Code = -99,
                        Message = "User is not authenticated",
                    };
                }
           
                var status = await _chatRepository.DeleteAsync(request.ChatID);

                if(status != 1){
                    return new APIResponse{
                        Code = status,
                        Message = "Delete chat failed",
                    };
                }

                return new APIResponse{
                    Code = 1,
                    Message = "Chat deleted successfully",
                };
               
            }
            catch(Exception ex){
                return new APIResponse{
                    Code = -99,
                    Message = "Delete chat failed: " + ex.Message,
                };
            }
        }
    }
}