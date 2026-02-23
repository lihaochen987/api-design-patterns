using backend.User.Controllers;
using backend.User.DomainModels;
using Mapster;

namespace backend.User.Services;

public static class UserMappingConfig
{
    public static void RegisterUserMappings(this TypeAdapterConfig config)
    {
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
