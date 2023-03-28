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

                user.First_name = entity.First_name;
                user.Last_name = entity.Last_name;
                user.Password = entity.Password;
                user.Email = entity.Email;
                user.Points = entity.Points;
                user.Rol_Id = entity.Rol_Id;
                user.Rol = entity.Rol;
                
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
