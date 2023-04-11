using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;

namespace Wallet_grupo1.DataAccess.DatabaseSeeding;

public class UserSeeder : IEntitySeeder
{
    public void SeedTheDatabase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User{ 
                Id = 1,
                FirstName = "Cotton" ,
                LastName = "Mather", 
                Email = "cm@gmail.com",
                Password = PasswordEncryptHelper.EncryptPassword("cm2k23"),
                Points = 1241,
                RoleId = 1
            },
            new User{ 
                Id = 2,
                FirstName = "Deodat" ,
                LastName = "Lawson", 
                Email = "dl@gmail.com",
                Password = PasswordEncryptHelper.EncryptPassword("dl2k23"),
                Points = 1242323,
                RoleId = 1
            },
            new User{ 
                Id = 3,
                FirstName = "Giles" ,
                LastName = "Corey", 
                Email = "gc@gmail.com",
                Password = PasswordEncryptHelper.EncryptPassword("ed2k23"),
                Points = 50,
                RoleId = 1
            },
            new User{ 
                Id = 4,
                FirstName = "James" ,
                LastName = "Bayley", 
                Email = "jb@gmail.com",
                Password = PasswordEncryptHelper.EncryptPassword("jb2k23"),
                Points = 50,
                RoleId = 2
            },
            new User{ 
                Id = 5,
                FirstName = "John" ,
                LastName = "Proctor", 
                Email = "jp@gmail.com",
                Password = PasswordEncryptHelper.EncryptPassword("jp2k23"),
                Points = 0,
                RoleId = 2
            },
            new User{ 
                Id = 6,
                FirstName = "Mary" ,
                LastName = "Eastey", 
                Email = "me@gmail.com",
                Password = PasswordEncryptHelper.EncryptPassword("me2k23"),
                Points = 50231,
                RoleId = 2
            }
        );
    }
}