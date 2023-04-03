using Wallet_grupo1.DataAccess.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.Repositories{

public class CatalogueRepository : Repository<Catalogue>, ICatalogueRepository
{
    public CatalogueRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<bool> Update(Catalogue entity)
        {
            try
            {
                var catalogue = await _context.Catalogues.FindAsync(entity.Id);

                // Si no se encontró ninguna entidad con ese ID no intento actualizar.
                if (catalogue is null) return false;

                catalogue.Image = entity.Image;
                catalogue.Points = entity.Points;

                _context.Catalogues.Update(catalogue);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

}

}