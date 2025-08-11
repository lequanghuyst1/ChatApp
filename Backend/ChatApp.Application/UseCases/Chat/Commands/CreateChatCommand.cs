using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using MediatR;
using System.Transactions;

namespace ChatApp.Application.UseCases.Chat.Commands
{
    public class CreateChatCommand : IRequest<APIResponse<long>>
    {
       public ChatType Type { get; set; }  // 1: Private, 2: Group
       public string Title { get; set; }  // Optional for private
       public long[] UserIDs { get; set; }  // Optional for private
    }

    public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, APIResponse<long>>
    {

        private readonly IChatRepository _chatRepository;
        private readonly IIdentityService _identityService;
        private readonly IChatParticipantRepository _chatParticipantRepository;
        
        public CreateChatCommandHandler(IChatRepository chatRepository, IIdentityService identityService, IChatParticipantRepository chatParticipantRepository)
        {
            _chatRepository = chatRepository;
            _identityService = identityService;
            _chatParticipantRepository = chatParticipantRepository;
        }
        
        public async Task<APIResponse<long>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            try{
                var userSession = _identityService.GetUser<UserProfile>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse<long> {
                        Code = -99,
                        Data = 0,
                        Message = "User is not authenticated",
                    };
                }

            using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var chat = new Domain.Entities.Chat{
                    Type = request.Type,
                    Title = request.Title,
                    CreatedBy = userSession.Data.UserID,
                    CreatedByName = $"{userSession.Data.FirstName} {userSession.Data.LastName}",
                };

                var result = await _chatRepository.CreateAsync(chat);

                if(result.status != 1){
                    return new APIResponse<long>{
                        Code = result.status,
                        Message = "Create chat failed",
                    };
                }

                foreach(var userId in request.UserIDs){

                    var role = userSession.Data.UserID == userId ? ChatParticipantType.ADMIN : ChatParticipantType.MEMBER;
                    
                    var chatParticipant = new ChatParticipant{
                        ChatID = result.id,
                        UserID = userId,
                        Role = role,
                    };

                    await _chatParticipantRepository.AddAsync(chatParticipant);
                }

                transaction.Complete();

                return new APIResponse<long>{
                    Code = 1,
                    Data = result.id,
                    Message = "Chat created successfully",
                };
              }
               
            }
            catch(Exception ex){
                return new APIResponse<long>{
                    Code = -99,
                    Message = "Create chat failed: " + ex.Message,
                };
            }
        }
    }
}