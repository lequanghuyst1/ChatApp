using MediatR;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Application.Interfaces;

namespace ChatApp.Application.UseCases{
    public class UpdateReactionCommand : IRequest<APIResponse>{
        public long ID { get; set; }
        public long MessageID { get; set; }
        public string Emoji { get; set; }
    }

    public class UpdateReactionCommandHandler : IRequestHandler<UpdateReactionCommand, APIResponse>{
        private readonly IReactionRepository _reactionRepository;
        private readonly IIdentityService _identityService;

        public UpdateReactionCommandHandler(IReactionRepository reactionRepository, IIdentityService identityService){
            _reactionRepository = reactionRepository;
            _identityService = identityService;
        }

        public async Task<APIResponse> Handle(UpdateReactionCommand request, CancellationToken cancellationToken){
            try{
                var userSession = _identityService.GetUser<UserProfile>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse{
                        Code = 0,
                        Message = "User is not authenticated",
                    };
                }

                var reaction = new Reaction{
                    ID = request.ID,
                    MessageID = request.MessageID,
                    UserID = userSession.Data.UserID,
                    Emoji = request.Emoji,
                };

                var result = await _reactionRepository.UpdateAsync(reaction);

                if(result != 1){
                    return new APIResponse{
                        Code = result,
                        Message = "Update reaction failed",
                    };
                }

                return new APIResponse{
                    Code = 1,
                    Message = "Update reaction success",
                };
            }
            catch(Exception ex){
                return new APIResponse{
                    Code = 0,
                    Message = "Update reaction failed: " + ex.Message,
                };
            }
        }
    }
}