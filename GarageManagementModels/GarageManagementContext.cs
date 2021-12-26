using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageManagementModels
{
    public class GarageManagementContext : DbContext
    {
        public GarageManagementContext(DbContextOptions options)
       : base(options)
        {

        }
        public DbSet<Car> Cars { get; set; } // Cars is table name, phan biet hoa thuong
        public DbSet<Garage> Garage { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Car>().Property(c => c.Id).ValueGeneratedOnAdd();
            //modelBuilder.Entity<Garage>().Property(g => g.Id).ValueGeneratedOnAdd();
            //new GarageMap(modelBuilder.Entity<Garage>());
        }
    }
}
