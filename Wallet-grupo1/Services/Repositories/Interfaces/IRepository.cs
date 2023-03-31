using Wallet_grupo1.Entities;

namespace Wallet_grupo1.Services.Repositories.Interfaces{

public interface IRepository<T> where T : class
{
    public Task<List<T>> GetAll();

    public Task<T?> GetById(int id);

    public Task<bool> Insert(T entity);

    public Task<bool> Delete(T entity);

    public Task<bool> Update(T entity);
}
}