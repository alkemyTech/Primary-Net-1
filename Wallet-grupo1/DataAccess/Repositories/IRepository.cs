using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.DataAccess.Repositories;

public interface IRepository<T> where T : class
{
    public Task<List<T>> GetAll();

    public Task<T?> GetById(int id);

    public void Insert(T entity);

    public void Delete(T entity);

    public void Update(T entity);
    Task<IEnumerable<Transaction>> GetAll();
}