using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.DataAccess.Repositories
{
    public class UserRepository : Repository<User>
    {
        
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public override async Task<bool> Update(User entity)
        {
            try
            {
                var user = await _context.Users.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return false;
                }

                user.first_name = entity.first_name;
                user.last_name = entity.last_name;
                user.password = entity.password;
                user.email = entity.email;
                user.points = entity.points;
                user.rol_Id = entity.rol_Id;
                
                _context.Users.Update(user);

                return true;

            }catch(Exception) { 

                return false;
            }
            
        }

        public override async Task<bool> Delete(User entity)
        {
            try
            {
                var user =  await _context.Users.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

                if (user != null)
                {
                    _context.Users.Remove(user);
                }
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

    }
}
