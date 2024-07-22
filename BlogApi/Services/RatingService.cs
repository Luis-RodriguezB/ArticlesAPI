using ArticlesAPI.DTOs.Rating;
using ArticlesAPI.Entities;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Repositories.Interfaces;
using ArticlesAPI.Services.Interfaces;
using AutoMapper;

namespace ArticlesAPI.Services;
public class RatingService : IRatingService
{
    private readonly IRatingRepository ratingRepository;
    private readonly IMapper mapper;

    public RatingService(IRatingRepository ratingRepository, IMapper mapper)
    {
        this.ratingRepository = ratingRepository;
        this.mapper = mapper;
    }

    public async Task Save(RatingCreateDTO entity)
    {
        if (await ratingRepository.Exist(entity.PersonId, entity.ArticleId))
        {
            throw new BadRequestException($"The person with the id {entity.PersonId}, already rating the article with the id {entity.ArticleId}");
        }

        if (entity.Like == entity.Dislike)
        {
            throw new BadRequestException("Like and Dislike can not have the same values");
        }

        var rating = mapper.Map<Rating>(entity);
        await ratingRepository.Save(rating);
    }

    public async Task Update(RatingCreateDTO entity)
    {
        if (entity.Like == entity.Dislike)
        {
            throw new BadRequestException("Like and Dislike can not have the same values");
        }
        var ratingDb = await ratingRepository.GetByIds(entity.PersonId, entity.ArticleId) 
            ?? throw new NotFoundException(
                $"The person with the id {entity.PersonId} has not yet rated the article with the id {entity.ArticleId}"
            );
        ratingDb = mapper.Map(entity, ratingDb);

        await ratingRepository.Update(ratingDb);
    }

    public async Task Delete(RatingDeleteDTO ratingDeleteDTO)
    {
        var rating = await ratingRepository.GetByIds(ratingDeleteDTO.PersonId, ratingDeleteDTO.ArticleId) 
            ?? throw new NotFoundException(
                $"The person with the id {ratingDeleteDTO.PersonId} has not yet rated the article with the id {ratingDeleteDTO.ArticleId}"
            );
        await ratingRepository.Delete(rating);
    }
}
