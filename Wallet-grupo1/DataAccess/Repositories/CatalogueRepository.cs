using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.DataAccess.Repositories;

public class CatalogueRepository : Repository<Catalogue>
{
    public CatalogueRepository(ApplicationDbContext context) : base(context)
    {
    }

}