using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using MediatR;
using System.Transactions;

namespace ChatApp.Application.UseCases.Chat.Commands
{
    public class UpdateChatCommand : IRequest<APIResponse>
    {
       public long ChatID { get; set; }
       public string Title { get; set; }  // Optional for private
    }

    public class UpdateChatCommandHandler : IRequestHandler<UpdateChatCommand, APIResponse>
    {

        private readonly IChatRepository _chatRepository;
        private readonly IIdentityService _identityService;
        
        public UpdateChatCommandHandler(IChatRepository chatRepository, IIdentityService identityService)
        {
            _chatRepository = chatRepository;
            _identityService = identityService;
        }
        
        public async Task<APIResponse> Handle(UpdateChatCommand request, CancellationToken cancellationToken)
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
           
                var chat = new ChatApp.Domain.Entities.Chat{
                    ID = request.ChatID,
                    Title = request.Title,
                    UpdatedBy = userSession.Data.UserID,
                    UpdatedByName = $"{userSession.Data.FirstName} {userSession.Data.LastName}",
                };

                var status = await _chatRepository.UpdateAsync(chat); 

                if(status != 1){
                    return new APIResponse{
                        Code = status,
                        Message = "Update chat failed",
                    };
                }

                return new APIResponse{
                    Code = 1,
                    Message = "Chat updated successfully",
                };
               
            }
            catch(Exception ex){
                return new APIResponse{
                    Code = -99,
                    Message = "Update chat failed: " + ex.Message,
                };
            }
        }
    }
}