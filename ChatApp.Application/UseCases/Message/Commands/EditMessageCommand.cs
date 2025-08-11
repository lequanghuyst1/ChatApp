using MediatR;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.UseCases{
    
    public class EditMessageCommand : IRequest<APIResponse>{
        public long ID { get; set; }
        public string Content { get; set; }
        public MessageType MessageType { get; set; }
    }

    public class EditMessageCommandHandler : IRequestHandler<EditMessageCommand, APIResponse>{

        private readonly IIdentityService _identityService;
        private readonly IMessageRepository _messageRepository;
        
        public EditMessageCommandHandler(IIdentityService identityService, IMessageRepository messageRepository){
            _identityService = identityService;
            _messageRepository = messageRepository;
        }

        public async Task<APIResponse> Handle(EditMessageCommand request, CancellationToken cancellationToken){
            try{
                var userSession = _identityService.GetUser<UserProfile>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse{
                        Code = 0,
                        Message = "User is not authenticated",
                    };
                }

                var message = new Message{
                    ID = request.ID,
                    Content = request.Content,
                    MessageType = request.MessageType,
                };

                var result = await _messageRepository.UpdateAsync(message);

                if(result != 1){
                    return new APIResponse{
                        Code = result,
                        Message = "Edit message failed",
                    };
                }

                return new APIResponse{
                    Code = 1,
                    Message = "Edit message successfully",
                };
            }
            catch(Exception ex){
                return new APIResponse{
                    Code = 0,
                    Message = "Edit message failed: " + ex.Message,
                };
            }
        }
    }
}
