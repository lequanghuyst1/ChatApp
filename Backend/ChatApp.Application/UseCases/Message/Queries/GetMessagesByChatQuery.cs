using MediatR;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Application.DTOs;
using AutoMapper;

namespace ChatApp.Application.UseCases{

    public class GetMessagesByChatResponse{
        public IEnumerable<MessageDTO> Messages { get; set; }
        public int TotalRec { get; set; }
    }

    public class GetMessagesByChatQuery : IRequest<APIResponse<GetMessagesByChatResponse>>{
        public long ChatID { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetMessagesByChatQueryHandler : IRequestHandler<GetMessagesByChatQuery, APIResponse<GetMessagesByChatResponse>>{
        private readonly IMessageRepository _messageRepository;
        private readonly IReactionRepository _reactionRepository;
        private readonly IMapper _mapper;

        public GetMessagesByChatQueryHandler(IMessageRepository messageRepository, IReactionRepository reactionRepository, IMapper mapper){
            _messageRepository = messageRepository;
            _reactionRepository = reactionRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<GetMessagesByChatResponse>> Handle(GetMessagesByChatQuery request, CancellationToken cancellationToken){
            try{
                var result = await _messageRepository.GetMessagesByChatIdAsync(request.ChatID, request.Page, request.PageSize);


                var messageDTOs = _mapper.Map<IEnumerable<MessageDTO>>(result.messages);

                foreach(var messageDTO in messageDTOs){
                    var reactions = await _reactionRepository.GetByMessageIdAsync(messageDTO.ID);

                    messageDTO.Reactions = reactions.Select(r => _mapper.Map<ReactionDTO>(r));
                }

                return new APIResponse<GetMessagesByChatResponse>{
                    Code = 1,
                    Data = new GetMessagesByChatResponse{Messages = messageDTOs, TotalRec = result.totalRec},
                    Message = "Get messages by chat success",
                };
            }

            catch(Exception ex){
                return new APIResponse<GetMessagesByChatResponse>{
                    Code = 0,
                    Message = "Get messages by chat failed: " + ex.Message,
                };
            }
        }

    }
}