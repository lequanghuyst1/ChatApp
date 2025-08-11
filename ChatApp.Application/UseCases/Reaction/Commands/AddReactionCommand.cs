using MediatR;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Application.Interfaces;

namespace ChatApp.Application.UseCases{
    public class AddReactionCommand : IRequest<APIResponse>{
        public long MessageID { get; set; }
        public string Emoji { get; set; }
    }

    public class AddReactionCommandHandler : IRequestHandler<AddReactionCommand, APIResponse>{
        private readonly IReactionRepository _reactionRepository;
        private readonly IIdentityService _identityService;

        public AddReactionCommandHandler(IReactionRepository reactionRepository, IIdentityService identityService){
            _reactionRepository = reactionRepository;
            _identityService = identityService;
        }

        public async Task<APIResponse> Handle(AddReactionCommand request, CancellationToken cancellationToken){
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
                    MessageID = request.MessageID,
                    UserID = userSession.Data.UserID,
                    Emoji = request.Emoji,
                };

                var result = await _reactionRepository.CreateAsync(reaction);

                if(result.status != 1){
                    return new APIResponse{
                        Code = result.status,
                        Message = "Add reaction failed",
                    };
                }

                return new APIResponse{
                    Code = 1,
                    Message = "Add reaction success",
                };
            }
            catch(Exception ex){
                return new APIResponse{
                    Code = 0,
                    Message = "Add reaction failed: " + ex.Message,
                };
            }
        }
    }
}