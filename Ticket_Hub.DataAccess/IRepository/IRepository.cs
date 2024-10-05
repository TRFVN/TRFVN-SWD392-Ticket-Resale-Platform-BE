﻿using System.Linq.Expressions;

namespace Ticket_Hub.DataAccess.IRepository;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}