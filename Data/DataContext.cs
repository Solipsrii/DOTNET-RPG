using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_RPG.Data
{
    public class DataContext : DbContext
    {
        //: base(x) is like java's super(x).
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill {id = 1, name = "fireball", damage = 15},
                new Skill {id = 2, name = "ice arrow", damage = 7},
                new Skill {id = 3, name = "lightning bolt", damage = 20},
                new Skill {id = 4, name = "earth lance", damage = 12}
            );
        }
        */

        //can save particular characters into table Characters in the sql-db, thanks to DbSet that abstracts that process.
        //note: "Characters" is considered an Entity. (of entity framework)
        //note: DbSet<class>, this "class" is used to define the Migration data model.
        public DbSet<Character> characters { get; set;}
        public DbSet<User> users { get; set;}
        public DbSet<Weapon> weapons {get; set;}
        public DbSet<Skill> skills { get; set; }
    }
}