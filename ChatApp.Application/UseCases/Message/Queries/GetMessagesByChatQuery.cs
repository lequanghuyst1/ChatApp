using MediatR;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Application.DTOs;
using AutoMapper;

namespace ChatApp.Application.UseCases{
    public class GetMessagesByChatQuery : IRequest<APIResponse<IEnumerable<MessageDTO>>>{
        public long ChatID { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetMessagesByChatQueryHandler : IRequestHandler<GetMessagesByChatQuery, APIResponse<IEnumerable<MessageDTO>>>{
        private readonly IMessageRepository _messageRepository;
        private readonly IReactionRepository _reactionRepository;
        private readonly IMapper _mapper;

        public GetMessagesByChatQueryHandler(IMessageRepository messageRepository, IReactionRepository reactionRepository, IMapper mapper){
            _messageRepository = messageRepository;
            _reactionRepository = reactionRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<IEnumerable<MessageDTO>>> Handle(GetMessagesByChatQuery request, CancellationToken cancellationToken){
            try{
                var result = await _messageRepository.GetMessagesByChatIdAsync(request.ChatID, request.Page, request.PageSize);


                var messageDTOs = _mapper.Map<IEnumerable<MessageDTO>>(result);

                foreach(var messageDTO in messageDTOs){
                    var reactions = await _reactionRepository.GetByMessageIdAsync(messageDTO.ID);

                    messageDTO.Reactions = reactions.Select(r => _mapper.Map<ReactionDTO>(r));
                }

                return new APIResponse<IEnumerable<MessageDTO>>{
                    Code = 1,
                    Data = messageDTOs,
                    Message = "Get messages by chat success",
                };
            }

            catch(Exception ex){
                return new APIResponse<IEnumerable<MessageDTO>>{
                    Code = 0,
                    Message = "Get messages by chat failed: " + ex.Message,
                };
            }
        }

    }
}