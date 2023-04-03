using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.Repositories{

public class CatalogueRepository : Repository<Catalogue>, ICatalogueRepository
{
    public CatalogueRepository(ApplicationDbContext context) : base(context)
    {
    }


        /// <summary>
        ///     Eliminacion de catalogue
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override async Task<bool> Delete(Catalogue entity)
        {
            try
            {
                var catalogue = await _context.Catalogues.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

                if (catalogue != null)
                {
                    _context.Catalogues.Remove(catalogue);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

    }

}