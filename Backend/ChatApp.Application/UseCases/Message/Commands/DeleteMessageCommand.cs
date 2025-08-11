using MediatR;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.UseCases{
    
    public class DeleteMessageCommand() : IRequest<APIResponse>{
        public long ID { get; set; }
    }

    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, APIResponse>{

        private readonly IIdentityService _identityService;
        private readonly IMessageRepository _messageRepository;
        
        public DeleteMessageCommandHandler(IIdentityService identityService, IMessageRepository messageRepository){
            _identityService = identityService;
            _messageRepository = messageRepository;
        }

        public async Task<APIResponse> Handle(DeleteMessageCommand request, CancellationToken cancellationToken){
            try{
                var userSession = _identityService.GetUser<UserProfile>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse{
                        Code = 0,
                        Message = "User is not authenticated",
                    };
                }

                var result = await _messageRepository.DeleteAsync(request.ID);

                if(result != 1){
                    return new APIResponse{
                        Code = result,
                        Message = "Delete message failed",
                    };
                }

                return new APIResponse{
                    Code = 1,
                    Message = "Delete message successfully",
                };
            }
            catch(Exception ex){
                return new APIResponse{
                    Code = 0,
                    Message = "Delete message failed: " + ex.Message,
                };
            }
        }
    }
}
