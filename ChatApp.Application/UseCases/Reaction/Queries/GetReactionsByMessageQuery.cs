using MediatR;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Application.Interfaces;
using ChatApp.Application.DTOs;

namespace ChatApp.Application.UseCases{
    public class GetReactionsByMessageQuery : IRequest<APIResponse<IEnumerable<ReactionDTO>>>
    {
        public long MessageID { get; set; }
    }

    public class GetReactionsByMessageQueryHandler : IRequestHandler<GetReactionsByMessageQuery, APIResponse<IEnumerable<ReactionDTO>>>
    {
        private readonly IReactionRepository _reactionRepository;

        public GetReactionsByMessageQueryHandler(IReactionRepository reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }

        public async Task<APIResponse<IEnumerable<ReactionDTO>>> Handle(GetReactionsByMessageQuery request, CancellationToken cancellationToken)
        {
            try{
                var reactions = await _reactionRepository.GetByMessageIdAsync(request.MessageID);
                
                var reactionDTOs = reactions.Select(r => mapToReactionDTO(r));

                return new APIResponse<IEnumerable<ReactionDTO>>{
                    Code = 1,
                    Data = reactionDTOs,
                    Message = "Get reactions by message success",
                };
            }
            catch(Exception ex){
                return new APIResponse<IEnumerable<ReactionDTO>>{
                    Code = 0,
                    Message = "Get reactions by message failed: " + ex.Message,
                };
            }
        }

        private ReactionDTO mapToReactionDTO(Reaction reaction){
            return new ReactionDTO{
                ID = reaction.ID,
                MessageID = reaction.MessageID,
                SenderID = reaction.SenderID,
                Emoji = reaction.Emoji,
                CreatedAt = reaction.CreatedAt
            };
        }
    }

}