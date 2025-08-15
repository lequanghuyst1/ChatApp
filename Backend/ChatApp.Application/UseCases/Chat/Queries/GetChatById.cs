using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Interfaces;
using MediatR;
using AutoMapper;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;

namespace ChatApp.Application.UseCases.Chat.Queries{

    public record GetChatByIdQuery(long id) : IRequest<APIResponse<ChatDTO>>{};
    
    public class GetChatByIdQueryHandler : IRequestHandler<GetChatByIdQuery, APIResponse<ChatDTO>>{
        private readonly IChatRepository _chatRepository;
        private readonly IIdentityService _identityService;
        private readonly IChatParticipantRepository _chatParticipantRepository;
        private readonly IMapper _mapper;
       
        public GetChatByIdQueryHandler(IChatRepository chatRepository, IIdentityService identityService, IChatParticipantRepository chatParticipantRepository, IMapper mapper){
            _chatRepository = chatRepository;
            _identityService = identityService;
            _chatParticipantRepository = chatParticipantRepository;
            _mapper = mapper;
        }
       
        public async Task<APIResponse<ChatDTO>> Handle(GetChatByIdQuery request, CancellationToken cancellationToken){
            try{
                var userSession = _identityService.GetUser<UserProfile>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse<ChatDTO>{
                        Code = 0,
                        Message = "User is not authenticated",
                    };
                }

                var result = await _chatRepository.GetByIdAsync(request.id);

                var participants = await _chatParticipantRepository.GetListByChatIdAsync(request.id);

                var participantsDTOs = _mapper.Map<IEnumerable<ChatParticipantDTO>>(participants);
                
                var chatDTOs = _mapper.Map<ChatDTO>(result);

                chatDTOs.Participants = participantsDTOs;
                
                return new APIResponse<ChatDTO>{
                    Code = 1,
                    Data = chatDTOs,
                    Message = "Get chat successfully",
                };
            }
            catch(Exception ex){
                return new APIResponse<ChatDTO>{
                    Code = 0,
                    Message = "Get chat failed: " + ex.Message,
                };
            }
        }
    }
}
            