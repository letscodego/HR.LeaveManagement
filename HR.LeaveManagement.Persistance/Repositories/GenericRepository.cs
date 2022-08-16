﻿using HR.LeaveManagement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly LeaveManagementDBContext _dbContext;
        public GenericRepository(LeaveManagementDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<T> Add(T entity)
        {
            await _dbContext.AddAsync(entity);
            return entity;
        }

        public async Task Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await Get(id);
            return entity != null;
        }

        public async Task<T> Get(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
