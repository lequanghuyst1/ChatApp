using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using MediatR;
using AutoMapper;

namespace ChatApp.Application.UseCases.Friend.Queries
{
    public record GetFriendListQuery() : IRequest<APIResponse<IEnumerable<FriendDTO>>>;

    public class GetFriendListQueryHandler : IRequestHandler<GetFriendListQuery, APIResponse<IEnumerable<FriendDTO>>>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetFriendListQueryHandler(IFriendRepository friendRepository, IIdentityService identityService, IMapper mapper)
        {
            _friendRepository = friendRepository;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<APIResponse<IEnumerable<FriendDTO>>> Handle(GetFriendListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userSession = _identityService.GetUser<UserSession>();

                if (userSession == null || userSession.Data == null)
                {
                    return new APIResponse<IEnumerable<FriendDTO>>{
                        Code = 0,
                        Message = "User is not authenticated",
                    };
                }
        
                var result = await _friendRepository.GetListByUserAsync(userSession.Data.UserID);

                var friendDTOs = _mapper.Map<IEnumerable<FriendDTO>>(result); 
        
                return new APIResponse<IEnumerable<FriendDTO>>{
                    Code = 1,
                    Data = friendDTOs,
                    Message = "Get friend list success",
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<FriendDTO>>{
                    Code = 0,
                    Message = "Get friend list failed: " + ex.Message,
                };
            }
        }
    }
}
