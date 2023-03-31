using Wallet_grupo1.DataAccess.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.Repositories{

public class CatalogueRepository : Repository<Catalogue>, ICatalogueRepository
{
    public CatalogueRepository(ApplicationDbContext context) : base(context)
    {
    }

}

}