using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Repositories;
using ChatApp.Utilities.Util;
using ChatApp.Utitilies.Security;
using FluentValidation;
using MediatR;

namespace ChatApp.Application.UseCases
{

    public record UpdateUserCommand : IRequest<APIResponse>
    {
        public bool IsActive { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastSeen { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, APIResponse>
    {

        private readonly IAccountRepository _accountRepository;
        private readonly IIdentityService _identityService;

        public UpdateUserCommandHandler(IAccountRepository accountRepository,  IIdentityService identityService)
        {
            _accountRepository = accountRepository;
            _identityService = identityService;
        }
        
        public async Task<APIResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userSession = _identityService.GetUser<UserProfile>();

                if (userSession == null)
                {
                    return new APIResponse
                    {
                        Code = -99,
                        Message = "User is not authenticated"
                    };
                }

                var account = new Domain.Entities.Account
                {
                    UserID = userSession.UserID,
                    IsActive = request.IsActive,
                    IsOnline = request.IsOnline,
                    LastSeen = request.LastSeen
                };
                
                var status = await _accountRepository.UpdateAsync(account);

                if (status != 1)
                {
                    return new APIResponse
                    {
                        Code = -99,
                        Message = "Update user failed"
                    };
                }

                return new APIResponse
                {
                    Code = 1,
                    Message = "Update user successfully"
                };

            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Code = -99,
                    Message = ex.Message
                };
            }
        }
    }
}