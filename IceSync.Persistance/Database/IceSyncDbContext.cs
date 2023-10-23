using IceSync.Persistance.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Persistance.Database
{
    public class IceSyncDbContext : DbContext
    {
        public IceSyncDbContext(DbContextOptions options) 
            : base(options) 
        {
        }

        protected Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();

        public DbSet<Workflow> Workflows { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(this.ConfigurationsAssembly);

            base.OnModelCreating(builder);
        }
    }
}
