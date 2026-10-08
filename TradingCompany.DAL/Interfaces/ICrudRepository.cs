namespace TradingCompany.DAL.Interfaces;

public interface ICrudRepository<T>
{
    IReadOnlyList<T> GetAll();

    T? GetById(int id);

    int Create(T entity);

    bool Update(T entity);

    bool Delete(int id);
}
