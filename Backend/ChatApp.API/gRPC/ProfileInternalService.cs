using Grpc.Core;
using ChatApp.API.Protos;
using ChatApp.Domain.Repositories;

namespace ChatApp.API.gRPC
{
    public class ProfileInternalService : ProfileService.ProfileServiceBase
    {
        private readonly ILogger<ProfileInternalService> _logger;
        private readonly IProfileRepository _profileRepository;

        public ProfileInternalService(ILogger<ProfileInternalService> logger, IProfileRepository profileRepository)
        {
            _logger = logger;
            _profileRepository = profileRepository;
        }
        public override async Task<ProfileProto> GetProfileByID(RequestByID request, ServerCallContext context)
        {
            try
            {
                _logger.LogInformation("Received request for profile with ID: {Id}", request.Id);

                var profile = await _profileRepository.GetProfileAsync(request.Id);

                var profileProto = new ProfileProto
                {
                    UserId = request.Id,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    FullName = profile.FirstName + profile.LastName,
                    Email = profile.Email,
                    Avatar = profile.Avatar,
                    Phone = profile.Phone,
                    Gender = (Gender)profile.Gender,
                    DateOfBirth = profile.DateOfBirth.ToString(),
                    Bio = profile.Bio
                };

                _logger.LogInformation("Returning profile for ID: {Id}", request.Id);

                return profileProto ?? new ProfileProto();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error: {error}", ex.Message);
                throw;
            }
        }
    }
}
