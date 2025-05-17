using backend.User.Controllers;
using backend.User.DomainModels;
using backend.User.DomainModels.ValueObjects;
using Mapster;

namespace backend.User.Services;

public static class UserMappingConfig
{
    public static void RegisterUserMappings(this TypeAdapterConfig config)
    {
        // Value objects
        config.NewConfig<FirstName, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, FirstName>()
            .MapWith(src => new FirstName(src));

        config.NewConfig<LastName, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, LastName>()
            .MapWith(src => new LastName(src));

        config.NewConfig<Email, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, Email>()
            .MapWith(src => new Email(src));

        // User mappings
        config.NewConfig<CreateUserRequest, DomainModels.User>();
        config.NewConfig<DomainModels.User, CreateUserResponse>();
        config.NewConfig<DomainModels.User, ReplaceUserResponse>();
        config.NewConfig<ReplaceUserRequest, DomainModels.User>();
        config.NewConfig<DomainModels.User, UpdateUserResponse>();
        config.NewConfig<UserView, GetUserResponse>();
        config.NewConfig<DomainModels.User, CreateUserRequest>();
        config.NewConfig<DomainModels.User, ReplaceUserRequest>();
    }
}
