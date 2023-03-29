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
        
        /// <summary>
        ///  Método para validar credenciales. Si son válidas devuelve el User correspondiente, si no, devuelve NULL.
        /// </summary>
        /// <param name="email">EMAIL correspondiente al User que se quiere loggear</param>
        /// <param name="pwd">pwd asociada a ese mail y User</param>
        /// <returns>User en caso de ser correctas las credenciales. NULL en caso de que no.</returns>
        public async Task<User?> AuthenticateCredentials(string email, string pwd)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == email && x.Password == pwd);
        }
        
    }
}
