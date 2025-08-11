using MediatR;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Application.Interfaces;

namespace ChatApp.Application.UseCases{
    public class RemoveReactionCommand : IRequest<APIResponse>{
        public long ID { get; set; }
     
    }

    public class RemoveReactionCommandHandler : IRequestHandler<RemoveReactionCommand, APIResponse>{
        private readonly IReactionRepository _reactionRepository;
        private readonly IIdentityService _identityService;

        public RemoveReactionCommandHandler(IReactionRepository reactionRepository, IIdentityService identityService){
            _reactionRepository = reactionRepository;
            _identityService = identityService;
        }

        public async Task<APIResponse> Handle(RemoveReactionCommand request, CancellationToken cancellationToken){
            try{
                var userSession = _identityService.GetUser<UserProfile>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse{
                        Code = 0,
                        Message = "User is not authenticated",
                    };
                }

                var result = await _reactionRepository.RemoveAsync(request.ID);

                if(result != 1){
                    return new APIResponse{
                        Code = result,
                        Message = "Remove reaction failed",
                    };
                }

                return new APIResponse{
                    Code = 1,
                    Message = "Remove reaction success",
                };
            }
            catch(Exception ex){
                return new APIResponse{
                    Code = 0,
                    Message = "Remove reaction failed: " + ex.Message,
                };
            }
        }
    }
}