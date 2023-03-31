using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public override async Task<bool> Delete(Role roleToDelete)
    {
        try
        {
            var role =  await _context.Roles.Where(role => role.Id == roleToDelete.Id).FirstOrDefaultAsync();

            if (role != null)
            {
                _context.Roles.Remove(role);
            }
        }
        catch(Exception)
        {
            return false;
        }
        return true;
    }
}