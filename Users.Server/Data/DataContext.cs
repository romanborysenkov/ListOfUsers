using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Users.Server.Models;

namespace Users.Server.Data
{
    public class DataContext: DbContext
        {
            public DataContext(DbContextOptions<DataContext>options):base(options)
            {  }
            public DbSet<User> Users{get; set;} 
        }
}