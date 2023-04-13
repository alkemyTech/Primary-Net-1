using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.Repositories;

/// <summary>
/// Controlador de entidad roles
/// </summary>
public class RoleRepository : Repository<Role>, IRoleRepository
{

    /// <summary>
    /// Constructor base
    /// </summary>
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Metodo de eliminacion del contexto de la API
    /// </summary>
    public override async Task<bool> Delete(Role roleToDelete)
    {
        try
        {
            var role = await _context.Roles.Where(role => role.Id == roleToDelete.Id).FirstOrDefaultAsync();

            if (role != null)
            {
                role.EsEliminado = true;
            }
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Metodo de actualizacion del contexto de la API
    /// </summary>
    public override async Task<bool> Update(Role roleToUpdate)
    {
        try
        {
            var role = await _context.Roles.Where(role => role.Id == roleToUpdate.Id).FirstOrDefaultAsync();
            if (role == null)
            {
                return false;
            }
          
            if (role.Id is 1 or 2)
            {
                role.Description = roleToUpdate.Description;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }
}