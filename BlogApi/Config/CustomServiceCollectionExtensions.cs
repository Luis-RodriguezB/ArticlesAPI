using ArticlesAPI.Repositories;
using ArticlesAPI.Repositories.Interfaces;
using ArticlesAPI.Services;
using ArticlesAPI.Services.Interfaces;
using BlogApi.Repositories;

namespace ArticlesAPI.Config;
public static class CustomServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        // REPOSITORIES
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // SERVICES
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IArticleCategoryService, ArticleCategoryService>();
        services.AddScoped<IUserArticleService, UserArticleService>();

        return services;
    }
}
