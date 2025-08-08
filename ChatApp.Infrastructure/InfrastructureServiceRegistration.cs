
using Microsoft.Extensions.DependencyInjection;
using ChatApp.Domain.Repositories;
using ChatApp.Infrastructure.Repositories;
using ChatApp.Domain.Interfaces;

namespace ChatApp.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IFriendRepository, FriendRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IChatParticipantRepository, ChatParticipantRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IReactionRepository, ReactionRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
        }
    }
}
