using MediatR;
using ChatApp.Domain.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Application.DTOs;
using AutoMapper;

namespace ChatApp.Application.UseCases{
    public class GetReactionsByMessageQuery : IRequest<APIResponse<IEnumerable<ReactionDTO>>>
    {
        public long MessageID { get; set; }
    }

    public class GetReactionsByMessageQueryHandler : IRequestHandler<GetReactionsByMessageQuery, APIResponse<IEnumerable<ReactionDTO>>>
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IMapper _mapper;

        public GetReactionsByMessageQueryHandler(IReactionRepository reactionRepository, IMapper mapper)
        {
            _reactionRepository = reactionRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<IEnumerable<ReactionDTO>>> Handle(GetReactionsByMessageQuery request, CancellationToken cancellationToken)
        {
            try{
                var reactions = await _reactionRepository.GetByMessageIdAsync(request.MessageID);
                
                var reactionDTOs = _mapper.Map<IEnumerable<ReactionDTO>>(reactions);

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
    }
}