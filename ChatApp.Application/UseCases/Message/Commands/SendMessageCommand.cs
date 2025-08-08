using MediatR;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.UseCases.Message.Commands{
    
    public class SendMessageCommand : IRequest<APIResponse<long>>{
        public long ChatID { get; set; }
        public long SenderID { get; set; }
        public string Content { get; set; }
        public MessageType MessageType { get; set; }
    }

    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, APIResponse<long>>{

        private readonly IIdentityService _identityService;
        private readonly IMessageRepository _messageRepository;
        
        public SendMessageCommandHandler(IIdentityService identityService, IMessageRepository messageRepository){
            _identityService = identityService;
            _messageRepository = messageRepository;
        }

        public async Task<APIResponse<long>> Handle(SendMessageCommand request, CancellationToken cancellationToken){
            try{
                var userSession = _identityService.GetUser<UserProfile>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse<long>{
                        Code = 0,
                        Message = "User is not authenticated",
                    };
                }

                var message = new Message{
                    ChatID = request.ChatID,
                    SenderID = request.SenderID,
                    Content = request.Content,
                    MessageType = request.MessageType,
                };

                var result = await _messageRepository.CreateAsync(message);

                return new APIResponse<long>{
                    Code = 1,
                    Data = result.id,
                    Message = "Send message successfully",
                };
            }
            catch(Exception ex){
                return new APIResponse<long>{
                    Code = 0,
                    Message = "Send message failed: " + ex.Message,
                };
            }
        }
    }
}
