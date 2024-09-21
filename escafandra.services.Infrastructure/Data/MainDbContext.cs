using escafandra.services.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Infrastructure.Data
{
    public class MainDbContext:DbContext
    {
        public DbSet<PDFDocument> PDFDocuments { get; set; } 

        public MainDbContext(DbContextOptions<MainDbContext> options):base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PDFDocument>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
