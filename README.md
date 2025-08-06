ChatApp.Solution/  # Tên solution (dotnet sln)
├── Domain/  # Project: ChatApp.Domain (classlib)
│   ├── Entities/  # Các entity từ DB
│   │   ├── Account.cs
│   │   ├── Profile.cs
│   │   ├── Friend.cs
│   │   ├── Chat.cs
│   │   ├── ChatParticipant.cs
│   │   ├── Message.cs
│   │   ├── Reaction.cs
│   │   ├── RefreshToken.cs
│   ├── Interfaces/  # Interfaces cho repositories (tách biệt để không phụ thuộc EF Core)
│   │   ├── IAccountRepository.cs
│   │   ├── IProfileRepository.cs
│   │   ├── IFriendRepository.cs
│   │   ├── IChatRepository.cs
│   │   ├── IChatParticipantRepository.cs
│   │   ├── IMessageRepository.cs
│   │   ├── IReactionRepository.cs
│   │   ├── IRefreshTokenRepository.cs
│   │   ├── IUnitOfWork.cs  # Interface cho Unit of Work
│   ├── Domain.csproj
├── Application/  # Project: ChatApp.Application (classlib)
│   ├── Dtos/  # Data Transfer Objects cho request/response
│   │   ├── AccountDto.cs
│   │   ├── ProfileDto.cs
│   │   ├── FriendDto.cs
│   │   ├── ChatDto.cs
│   │   ├── MessageDto.cs
│   │   ├── ReactionDto.cs
│   │   ├── RegisterDto.cs
│   │   ├── LoginDto.cs
│   │   ├── RefreshTokenDto.cs
│   │   ├── AuthResponseDto.cs
│   ├── Validators/  # Các validator cho DTOs hoặc commands
│   │   ├── RegisterDtoValidator.cs
│   │   ├── LoginDtoValidator.cs
│   │   ├── RefreshTokenDtoValidator.cs
│   │   ├── UpdateProfileDtoValidator.cs
│   ├── UseCases/  # Các use cases (CQRS với MediatR)
│   │   ├── Account/  # Use cases liên quan đến tài khoản
│   │   │   ├── Commands/  # Commands cho mutate data (create/update/delete)
│   │   │   │   ├── RegisterUserCommand.cs
│   │   │   │   ├── RegisterUserCommandHandler.cs
│   │   │   │   ├── LoginUserCommand.cs
│   │   │   │   ├── LoginUserCommandHandler.cs
│   │   │   │   ├── RefreshTokenCommand.cs
│   │   │   │   ├── RefreshTokenCommandHandler.cs
│   │   │   ├── Queries/  # Queries cho read data
│   │   │   │   ├── GetUserByIdQuery.cs
│   │   │   │   ├── GetUserByIdQueryHandler.cs
│   │   │   │   ├── SearchUsersQuery.cs
│   │   │   │   ├── SearchUsersQueryHandler.cs
│   │   ├── Profile/  # Use cases liên quan đến hồ sơ
│   │   │   ├── Commands/
│   │   │   │   ├── UpdateProfileCommand.cs
│   │   │   │   ├── UpdateProfileCommandHandler.cs
│   │   │   │   ├── UploadAvatarCommand.cs
│   │   │   │   ├── UploadAvatarCommandHandler.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetProfileByIdQuery.cs
│   │   │   │   ├── GetProfileByIdQueryHandler.cs
│   │   ├── Friend/  # Use cases liên quan đến bạn bè
│   │   │   ├── Commands/
│   │   │   │   ├── SendFriendRequestCommand.cs
│   │   │   │   ├── SendFriendRequestCommandHandler.cs
│   │   │   │   ├── AcceptFriendRequestCommand.cs
│   │   │   │   ├── AcceptFriendRequestCommandHandler.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetFriendsQuery.cs
│   │   │   │   ├── GetFriendsQueryHandler.cs
│   │   ├── Chat/  # Use cases liên quan đến chat
│   │   │   ├── Commands/
│   │   │   │   ├── CreateChatCommand.cs
│   │   │   │   ├── CreateChatCommandHandler.cs
│   │   │   │   ├── AddParticipantCommand.cs
│   │   │   │   ├── AddParticipantCommandHandler.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetChatsByUserQuery.cs
│   │   │   │   ├── GetChatsByUserQueryHandler.cs
│   │   ├── Message/  # Use cases liên quan đến tin nhắn
│   │   │   ├── Commands/
│   │   │   │   ├── SendMessageCommand.cs
│   │   │   │   ├── SendMessageCommandHandler.cs
│   │   │   │   ├── EditMessageCommand.cs
│   │   │   │   ├── EditMessageCommandHandler.cs
│   │   │   │   ├── DeleteMessageCommand.cs
│   │   │   │   ├── DeleteMessageCommandHandler.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetMessagesByChatQuery.cs
│   │   │   │   ├── GetMessagesByChatQueryHandler.cs
│   │   ├── Reaction/  # Use cases liên quan đến reaction
│   │   │   ├── Commands/
│   │   │   │   ├── AddReactionCommand.cs
│   │   │   │   ├── AddReactionCommandHandler.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetReactionsByMessageQuery.cs
│   │   │   │   ├── GetReactionsByMessageQueryHandler.cs
│   ├── Behaviors/  # Pipeline behaviors cho MediatR
│   │   ├── ValidationBehavior.cs  # Tích hợp FluentValidation
│   │   ├── LoggingBehavior.cs  # Logging cho commands/queries
│   │   ├── AuthorizationBehavior.cs  # Kiểm tra quyền nếu cần
│   ├── Interfaces/  # Interfaces cho application (nếu cần, ví dụ IEmailService)
│   │   ├── IEmailService.cs  # Ví dụ: gửi email xác thực
│   ├── Application.csproj
├── Infrastructure/  # Project: ChatApp.Infrastructure (classlib)
│   ├── Data/  # DbContext và migrations
│   │   ├── ChatDbContext.cs
│   │   ├── Configurations/  # Cấu hình entity (fluent API)
│   │   │   ├── AccountConfiguration.cs
│   │   │   ├── ProfileConfiguration.cs
│   │   │   ├── RefreshTokenConfiguration.cs
│   ├── Repositories/  # Triển khai repositories
│   │   ├── AccountRepository.cs
│   │   ├── ProfileRepository.cs
│   │   ├── FriendRepository.cs
│   │   ├── ChatRepository.cs
│   │   ├── ChatParticipantRepository.cs
│   │   ├── MessageRepository.cs
│   │   ├── ReactionRepository.cs
│   │   ├── RefreshTokenRepository.cs
│   │   ├── UnitOfWork.cs  # Triển khai IUnitOfWork
│   ├── Services/  # Triển khai dịch vụ bên thứ ba
│   │   ├── EmailService.cs  # Ví dụ: gửi email
│   │   ├── FileStorageService.cs  # Ví dụ: upload avatar/media
│   ├── Infrastructure.csproj
├── Presentation/  # Project: ChatApp.Presentation (webapi)
│   ├── Controllers/  # API controllers
│   │   ├── AccountController.cs
│   │   ├── ProfileController.cs
│   │   ├── FriendController.cs
│   │   ├── ChatController.cs
│   │   ├── MessageController.cs
│   │   ├── ReactionController.cs
│   ├── Hubs/  # SignalR hubs cho real-time
│   │   ├── ChatHub.cs  # Xử lý gửi tin nhắn, trạng thái online, reaction
│   ├── Middlewares/  # Custom middlewares nếu cần
│   │   ├── ExceptionMiddleware.cs  # Xử lý lỗi toàn cầu
│   ├── Extensions/  # Extension methods cho dependency injection
│   │   ├── ServiceCollectionExtensions.cs  # Đăng ký DI
│   ├── Program.cs  # Điểm khởi chạy, cấu hình DI, middleware
│   ├── Startup.cs  # Nếu dùng startup riêng (optional)
│   ├── appsettings.json  # Cấu hình (DB connection, JWT key, etc.)
│   ├── appsettings.Development.json
│   ├── Presentation.csproj
├── Tests/  # Project: ChatApp.Tests (xunit or nunit)
│   ├── UnitTests/
│   │   ├── ApplicationTests/  # Test handlers và validators
│   │   │   ├── RegisterUserCommandHandlerTests.cs
│   │   ├── DomainTests/  # Test entities nếu cần
│   │   ├── InfrastructureTests/  # Test repositories
│   │   ├── PresentationTests/  # Test controllers
│   ├── IntegrationTests/  # Test API endpoints
│   ├── Tests.csproj
├── ChatApp.sln  # Solution file
├── README.md  # Hướng dẫn dự án
├── .gitignore