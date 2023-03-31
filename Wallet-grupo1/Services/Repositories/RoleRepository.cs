using Wallet_grupo1.Entities;
using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.Services.Repositories.Interfaces;

namespace Wallet_grupo1.Services.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
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