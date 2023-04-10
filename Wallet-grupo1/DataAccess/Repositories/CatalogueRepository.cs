using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.Repositories{
    
    /// <summary>
    /// Clase repositorio para gestionar las instancias de entidades de unidades Catálogo en el marco de la aplicación
    /// en conjunción con el motor de la base de datos, mediante operaciones asíncronas. Se utiliza el contexto provisto
    /// por la librería de Entity Framework para la persistencia y el manejo de operaciones SQL.
    /// </summary>
    public class CatalogueRepository : Repository<Catalogue>, ICatalogueRepository
{
    /// <summary>
    /// Constructor base del repositorio de catálogos
    /// </summary>
    public CatalogueRepository(ApplicationDbContext context) : base(context)
    {
    }
    
        /// <summary>
        /// Operación CRUD para remover un catálogo del arreglo interno de la aplicación y el contexto de la DB
        /// </summary>
        /// <param name="catalogueToDelete"></param>
        /// <returns>Booleano representativo del éxito o fracaso de la operación</returns>
        public override async Task<bool> Delete(Catalogue catalogueToDelete)
        {
            try
            {
                var catalogue = await _context.Catalogues.Where(x => x.Id == catalogueToDelete.Id).FirstOrDefaultAsync();

                if (catalogue != null)
                {
                    _context.Catalogues.Remove(catalogue);
                }
            }
            catch (Exception)
            {
                return false;
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