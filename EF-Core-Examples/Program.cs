using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppContext())
            {
                var folder = new Folder() { Name = "hello" };
                context.Set<Folder>().Add(folder);

                context.SaveChanges();
            }

            using (var context = new AppContext())
            {
                try
                {
                    var folder = context.Set<Folder>().FirstOrDefault();

                    ThisDeleteIsAbsolutelyInsane_ButProvesAPoint(folder.Id);

                    context.Set<Folder>().Remove(folder);

                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        var f = (Folder)entry.Entity;

                        var doesFolderNoLongerExist = context
                            .Set<Folder>()
                            .AsNoTracking()
                            .SingleOrDefault(fo => fo.Id == f.Id) == null;

                        if (doesFolderNoLongerExist)
                        {
                            return;
                        }

                        context.Set<Folder>().Remove(f);

                        context.SaveChanges();
                    }
                }
            }
        }

        static void ThisDeleteIsAbsolutelyInsane_ButProvesAPoint(Guid guid)
        {
            using (var context = new AppContext())
            {
                var folder = context.Set<Folder>().SingleOrDefault(f => f.Id == guid);

                context.Set<Folder>().Remove(folder);

                context.SaveChanges();
            }
        }
    }

    class AppContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=MyDb.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<Folder>();

            entityBuilder.HasKey(f => f.Id);

            entityBuilder.Property(f => f.Id)
                .ValueGeneratedOnAdd()
                .IsConcurrencyToken();
        }
    }

    class Folder
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
