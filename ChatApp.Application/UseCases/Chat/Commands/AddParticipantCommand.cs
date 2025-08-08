using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using MediatR;
using System.Transactions;

namespace ChatApp.Application.UseCases.Chat.Commands{

    // public class AddParticipantCommandValidator : AbstractValidator<AddParticipantCommand>
    // {
    //     public AddParticipantCommandValidator()
    //     {
    //         RuleFor(x => x.ChatID).GreaterThan(0);
    //         RuleFor(x => x.UserIDs).Must(ids => ids != null && ids.Length > 0).WithMessage("User IDs are required");
    //         RuleFor(x => x.UserIDs).Must(ids => ids.All(id => id > 0)).WithMessage("Invalid user IDs");
    //     }
    // }

    public class AddParticipantCommand : IRequest<APIResponse>
    {
        public long ChatID { get; set; }
        public long[] UserIDs { get; set; }
    }

    public class AddParticipantCommandHandler : IRequestHandler<AddParticipantCommand, APIResponse>
    {

        private readonly IIdentityService _identityService;
        private readonly IChatParticipantRepository _chatParticipantRepository;
            
        public AddParticipantCommandHandler(IIdentityService identityService, IChatParticipantRepository chatParticipantRepository)
        {
            _identityService = identityService;
            _chatParticipantRepository = chatParticipantRepository;
        }
        
        public async Task<APIResponse> Handle(AddParticipantCommand request, CancellationToken cancellationToken)
        {

            try{
                var userSession = _identityService.GetUser<UserProfile>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse{
                        Code = 0,
                        Message = "User is not authenticated",
                    };
                }

                using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach(var userId in request.UserIDs){
                        var chatParticipant = new ChatParticipant{
                            ChatID = request.ChatID,
                            UserID = userId,
                            Role = ChatParticipantType.MEMBER,
                        };
                        
                        var result = await _chatParticipantRepository.AddAsync(chatParticipant);

                        if(result.status != 1){
                            return new APIResponse{
                                Code = result.status,
                                Message = "Add participant failed",
                            };
                        }
                    }

                    transaction.Complete(); 

                    return new APIResponse{
                        Code = 1,
                        Message = "Add participant successfully",
                    };
                }
            }
            catch(Exception ex){
                return new APIResponse{
                    Code = -99,
                    Message = "Add participant failed: " + ex.Message,
                };
            }
        }
    }
}