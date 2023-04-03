using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<bool> Update(User updatedUser)
        {
            try
            {
                var existingUser = await _context.Users.Where(x => x.Id == updatedUser.Id).FirstOrDefaultAsync();

                if (existingUser == null)
                {
                    return false;
                }

                existingUser.FirstName = updatedUser.FirstName;
                existingUser.LastName = updatedUser.LastName;
                existingUser.Password = updatedUser.Password;
                existingUser.Email = updatedUser.Email;

                _context.Users.Update(existingUser);

                return true;

            }
            catch (Exception)
            {

                return false;
            }

        }

        public override async Task<bool> Delete(User entity)
        {
            try
            {
                var user = await _context.Users.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

                if (user != null)
                {
                    _context.Users.Remove(user);
                }
            }
            catch (Exception)
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

        /// <summary>
        /// Método encargado de validar que no exista ningun usario en la BD que contenta el mismo email.
        /// </summary>
        /// <param name="user">Usuario el cual se quiera validar que no exista en la BD</param>
        /// <returns>bool: true si existe, false si no existe</returns>
        public async Task<bool> ExisteUsuario(User user)
        {
            return await _context.Users
                .Where(x => x.Email == user.Email)
                .AnyAsync();
        }

    }
}
