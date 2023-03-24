using Wallet_grupo1.DataAccess.Repositories;

namespace Wallet_grupo1.Services;

public interface IUnitOfWork : IDisposable
{
    //TODO: Agregar los getters de cada interfaz de Repository concreto.
    int complete();

    public UserRepository UserRepo { get; }
}