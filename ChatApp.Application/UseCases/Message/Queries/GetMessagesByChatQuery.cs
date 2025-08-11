using MediatR;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Application.DTOs;

namespace ChatApp.Application.UseCases{
    public class GetMessagesByChatQuery : IRequest<APIResponse<IEnumerable<MessageDTO>>>{
        public long ChatID { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetMessagesByChatQueryHandler : IRequestHandler<GetMessagesByChatQuery, APIResponse<IEnumerable<MessageDTO>>>{
        private readonly IMessageRepository _messageRepository;

        public GetMessagesByChatQueryHandler(IMessageRepository messageRepository){
            _messageRepository = messageRepository;
        }

        public async Task<APIResponse<IEnumerable<MessageDTO>>> Handle(GetMessagesByChatQuery request, CancellationToken cancellationToken){
            try{
                var result = await _messageRepository.GetMessagesByChatIdAsync(request.ChatID, request.Page, request.PageSize);


                var messageDTOs = result.Select(m => mapToMessageDTO(m));

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

        private MessageDTO mapToMessageDTO(Message message){
            return new MessageDTO{
                ID = message.ID,
                ChatID = message.ChatID,
                SenderID = message.SenderID,
                Content = message.Content,
                MessageType = message.MessageType,
                CreatedAt = message.CreatedAt,
                IsEdited = message.IsEdited,
                EditedAt = message.EditedAt,
                IsDeleted = message.IsDeleted,
                DeletedAt = message.DeletedAt,
                ReadBy = message.ReadBy,
            };
        }
    }
}