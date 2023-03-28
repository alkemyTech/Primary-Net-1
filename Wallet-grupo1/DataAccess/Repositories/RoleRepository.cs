using Wallet_grupo1.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Wallet_grupo1.DataAccess.Repositories;

public class RoleRepository : Repository<Role>
{
    
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public override async Task<bool> Delete(Role roleToDelete)
    {
        try
        {
            var user =  await _context.Roles.Where(role => role.Id == roleToDelete.Id).FirstOrDefaultAsync();

            if (roleToDelete != null)
            {
                _context.Roles.Remove(roleToDelete);
            }
        }
        catch(Exception)
        {
            return false;
        }
        return true;
    }
}