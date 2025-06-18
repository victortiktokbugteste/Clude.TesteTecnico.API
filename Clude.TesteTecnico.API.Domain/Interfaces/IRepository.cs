using Clude.TesteTecnico.API.Domain.Entities;
using System.Linq.Expressions;

namespace Clude.TesteTecnico.API.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<T> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<List<T>> GetAllAsync();
    }
} 