using Microsoft.EntityFrameworkCore;
using NASATechAPI.Entities;

namespace NASATechAPI.DbContexts
{
    public class SQLServerConnection : DbContext
    {
        public SQLServerConnection(DbContextOptions<SQLServerConnection> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRoles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(c => c.Id);
                //entity.Property(e => e.Descripcion)
                //    .IsRequired()
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.HasOne(d => d.IdPadreNavigation)
                //    .WithMany(p => p.InverseIdPadreNavigation)
                //    .HasForeignKey(d => d.IdPadre)
                //    .HasConstraintName("FK_Categorias_Categorias");
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.HasKey(c => c.Id);
            });

        }
    }
}
