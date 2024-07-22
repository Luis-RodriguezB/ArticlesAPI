using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.DTOs.Others;
using ArticlesAPI.Helpers;
using ArticlesAPI.Repositories.Interfaces;
using ArticlesAPI.Utils;
using BlogApi;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ArticlesAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext context;

    public UserRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<User>> GetAll(UserFilter userFilter)
    {
        IQueryable<User> queryable = GetPersonQueryableFilter(userFilter);
        return await GetAll(userFilter, queryable);
    }

    public async Task<IEnumerable<User>> GetAll(UserFilter userFilter, IQueryable<User> queryable)
    {
        PaginationDTO paginationDTO = Utilities.GetPaginationDTO(userFilter);
        return await queryable.Paginate(paginationDTO).ToListAsync();
    }  

    public async Task<User> GetById(string id)
    {
        return await GetQueryable().FirstOrDefaultAsync(x => x.Id == id);
    }

    private IQueryable<User> GetPersonQueryableFilter(UserFilter userFilter)
    {
        IQueryable<User> queryable = GetQueryable();

        ApplyFilters(userFilter, ref queryable);
        ApplySorting(userFilter, ref queryable);

        return queryable;
    }

    private static void ApplyFilters(UserFilter userFilter, ref IQueryable<User> queryable)
    {
        if (userFilter == null) return;

        if (!string.IsNullOrEmpty(userFilter.Username))
        {
            queryable = queryable.Where(x => x.UserName.Contains(userFilter.Username));
        }

        if (!string.IsNullOrEmpty(userFilter.Name))
        {
            queryable = queryable.Where(x => x.Person.FirstName.Contains(userFilter.Name));
        }

        if (!string.IsNullOrEmpty(userFilter.LastName))
        {
            queryable = queryable.Where(x => x.Person.LastName.Contains(userFilter.LastName));
        }
    }

    private static void ApplySorting(UserFilter userFilter, ref IQueryable<User> queryable)
    {
        var isOrderAsc = userFilter.OrderBy?.Equals("asc", StringComparison.OrdinalIgnoreCase) ?? true;
        var typeOrder = userFilter.TypeOrder?.ToLower();

        queryable = typeOrder switch
        {
            "username" => ApplyOrder(queryable, x => x.UserName, isOrderAsc),
            "name" => ApplyOrder(queryable, x => x.Person.FirstName, isOrderAsc),
            "lastname" => ApplyOrder(queryable, x => x.Person.LastName, isOrderAsc),
            "email" => ApplyOrder(queryable, x => x.Email, isOrderAsc),
            _ => ApplyOrder(queryable, x => x.UserName, isOrderAsc)
        };
    }

    private static IQueryable<T> ApplyOrder<T, TKey>(IQueryable<T> queryable, Expression<Func<T, TKey>> keySelector, bool isOrderAsc)
    {
        return isOrderAsc ? queryable.OrderBy(keySelector) : queryable.OrderByDescending(keySelector);
    }

    private IQueryable<User> GetQueryable()
    {
        return context.Users
            .Where(u => !u.Email.Contains("admin@admin.com"))
            .AsNoTracking()
            .AsQueryable();
    }
}
