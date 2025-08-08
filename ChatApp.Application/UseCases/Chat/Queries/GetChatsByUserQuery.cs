using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.UseCases.Chat.Queries{

    public record GetChatsByUserQuery() : IRequest<APIResponse<IEnumerable<ChatDTO>>>;
    
    public class GetChatsByUserQueryHandler : IRequestHandler<GetChatsByUserQuery, APIResponse<IEnumerable<ChatDTO>>>{
        private readonly IChatRepository _chatRepository;
        private readonly IIdentityService _identityService;
        private readonly IChatParticipantRepository _chatParticipantRepository;
       
        public GetChatsByUserQueryHandler(IChatRepository chatRepository, IIdentityService identityService, IChatParticipantRepository chatParticipantRepository){
            _chatRepository = chatRepository;
            _identityService = identityService;
            _chatParticipantRepository = chatParticipantRepository;
        }
       
        public async Task<APIResponse<IEnumerable<ChatDTO>>> Handle(GetChatsByUserQuery request, CancellationToken cancellationToken){
            try{
                var userSession = _identityService.GetUser<UserProfile>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse<IEnumerable<ChatDTO>>{
                        Code = 0,
                        Message = "User is not authenticated",
                    };
                }

                var result = await _chatRepository.GetListByUserAsync(userSession.Data.UserID);

                var participantIds = await _chatParticipantRepository.GetListByChatIdAsync(userSession.Data.UserID);

                var chatDTOs = result.Select(chat => new ChatDTO{
                    ID = chat.ID,
                    Type = chat.Type,
                    Title = chat.Title,
                    CreatedBy = chat.CreatedBy,
                    CreatedByName = chat.CreatedByName,
                    CreatedAt = chat.CreatedAt,
                    UpdatedBy = chat.UpdatedBy,
                    UpdatedByName = chat.UpdatedByName,
                    UpdatedAt = chat.UpdatedAt,
                    IsDeleted = chat.IsDeleted,
                    ParticipantIds = participantIds.Select(p => p.UserID).ToList(),
                });
                
                return new APIResponse<IEnumerable<ChatDTO>>{
                    Code = 1,
                    Data = chatDTOs,
                    Message = "Get chat list successfully",
                };
            }
            catch(Exception ex){
                return new APIResponse<IEnumerable<ChatDTO>>{
                    Code = 0,
                    Message = "Get chat list failed: " + ex.Message,
                };
            }
        }
    }
}