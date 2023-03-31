using Wallet_grupo1.Entities;
using Wallet_grupo1.Services.Repositories.Interfaces;

namespace Wallet_grupo1.Services.Repositories{

public class CatalogueRepository : Repository<Catalogue>, ICatalogueRepository
{
    public CatalogueRepository(ApplicationDbContext context) : base(context)
    {
    }

}

}