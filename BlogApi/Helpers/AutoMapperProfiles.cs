using ArticlesAPI.DTOs.Article;
using ArticlesAPI.DTOs.Auth;
using ArticlesAPI.DTOs.Category;
using ArticlesAPI.DTOs.Person;
using ArticlesAPI.Entities;
using AutoMapper;
using BlogApi.DTOs.Auth;
using BlogApi.DTOs.Blog;
using BlogApi.Models;

namespace BlogApi.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Article, ArticleDTO>()
            .ForMember(a => a.Person, options => options.MapFrom(MapPersonArticle))
            .ForMember(a => a.Categories, options => options.MapFrom(MapCategoriesArticle));
        CreateMap<ArticleCreateDTO, Article>();
        CreateMap<ArticleUpdateDTO, Article>();

        CreateMap<RegisterDTO, Person>();
        CreateMap<PersonUpdateDTO, Person>();
        CreateMap<Person, PersonDTO>()
            .ForMember(p => p.Email, option => option.MapFrom(src => src.User.Email))
            .ForMember(p => p.Articles, options => options.MapFrom(MapArticlesPerson));
        CreateMap<Person, PersonUserDTO>();
        CreateMap<User, UserDTO>()
            .ForMember(u => u.Person, opt => opt.MapFrom(sr => sr.Person));

        CreateMap<Category, CategoryDTO>();
        CreateMap<CategoryCreateDTO, Category>();
        CreateMap<Category, CategoryArticleDTO>();
    }

    private List<CategoryDTO> MapCategoriesArticle(Article article, ArticleDTO articleDTO)
    {
        var categories = new List<CategoryDTO>();

        if (article.ArticleCategories == null)
        {
            return categories;
        }

        foreach (var category in article.ArticleCategories)
        {
            categories.Add(new CategoryDTO
            {
                Id = category.CategoryId,
                Name = category.Category.Name
            });
        }

        return categories;
    }
    private List<ArticlePersonDTO> MapArticlesPerson(Person person, PersonDTO personDTO)
    {
        var articles = new List<ArticlePersonDTO>();

        if (person.Articles == null)
        {
            return articles;
        }

        foreach (var article in person.Articles)
        {
            articles.Add(new ArticlePersonDTO()
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,
                Categories = article.ArticleCategories.Select(x => new CategoryDTO()
                {
                    Id = x.CategoryId,
                    Name = x.Category.Name,
                }).ToList()
            });
        }

        return articles;
    }
    private PersonArticleDTO MapPersonArticle(Article article, ArticleDTO articleDTO)
    {
        var personArticle = new PersonArticleDTO()
        {
            Id = article.PersonId,
            Email = article.Person.User.Email,
            FirstName = article.Person.FirstName,
            LastName = article.Person.LastName,
        };

        return personArticle;
    }
}
