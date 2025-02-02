﻿using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.DTOs.Others;
using ArticlesAPI.Helpers;
using ArticlesAPI.Repositories.Interfaces;
using ArticlesAPI.Utils;
using BlogApi;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticlesAPI.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly ApplicationDbContext _context;

    public PersonRepository(ApplicationDbContext context)
    {
        this._context = context;
    }
    public async Task<IEnumerable<Person>> GetAll()
    {
        return await _context.People
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Person>> GetAll(PersonFilter personFilter)
    {
        IQueryable<Person> queryable = GetPersonQueryableFilter(personFilter);
        return await GetAll(personFilter, queryable);
    }

    public async Task<IEnumerable<Person>> GetAll(PersonFilter personFilter, IQueryable<Person> queryable)
    {
        PaginationDTO paginationDTO = Utilities.GetPaginationDTO(personFilter);
        return await queryable.Paginate(paginationDTO).ToListAsync();
    }

    public async Task<Person> GetById(int id)
    {
        return await _context.People
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Person> GetPersonByUserId(string userId)
    {
        return await _context.People
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<Person> Save(Person entity)
    {
        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;

        _context.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task Update(Person person)
    {
        person.UpdatedAt = DateTime.Now;
        _context.Entry(person).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var exist = await Exist(id);
        if (exist)
        {
            _context.Remove(new Person { Id = id });
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.People.AnyAsync(x => x.Id == id);
    }

    private IQueryable<Person> GetPersonQueryableFilter(PersonFilter personFilter)
    {
        IQueryable<Person> queryable = _context.People.AsNoTracking();

        if (personFilter == null) return queryable;

        if (!string.IsNullOrEmpty(personFilter.Name))
        {
            queryable = queryable.Where(x => x.FirstName.Contains(personFilter.Name));
        }

        if (!string.IsNullOrEmpty(personFilter.LastName))
        {
            queryable = queryable.Where(x => x.LastName.Contains(personFilter.LastName));
        }

        if (!string.IsNullOrEmpty(personFilter.OrderBy))
        {
            var isOrderAsc = personFilter.OrderBy.Equals("asc", StringComparison.OrdinalIgnoreCase);
            var isNameOrder = personFilter.TypeOrder?.Equals("name", StringComparison.OrdinalIgnoreCase) ?? true;

            if (isNameOrder)
            {
                queryable = isOrderAsc
                    ? queryable.OrderBy(x => x.FirstName)
                    : queryable.OrderByDescending(x => x.FirstName);
            }
            else
            {
                queryable = isOrderAsc
                    ? queryable.OrderBy(x => x.LastName)
                    : queryable.OrderByDescending(x => x.LastName);
            }
        }

        return queryable;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
