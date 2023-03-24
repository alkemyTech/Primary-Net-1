namespace Wallet_grupo1.DataAccess.Repositories;

public interface IRepository<T> where T : class
{
    public List<T> GetAll();

    public T? GetById(int id);

    public void Insert(T entity);

    public void Delete(T entity);

    public void Update(T entity);
}