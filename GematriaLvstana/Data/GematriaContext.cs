using GematriaLvistana.Models;
using Microsoft.EntityFrameworkCore;

namespace GematriaLvistana.Data
{
    //for sql server
    public class GematriaContext : DbContext
    {
        public GematriaContext(DbContextOptions<GematriaContext> options) : base(options)
        {
            //use SQL Server
             
            Database.EnsureCreated();


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<WordModel>().ToTable("Words");
        }
        public DbSet<WordModel> Words { get; set; } = null!;

    }
}
