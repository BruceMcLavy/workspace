using HQV.Models;
using Microsoft.EntityFrameworkCore;

namespace HQV.Data;
public class AppDb(DbContextOptions<AppDb> opts) : DbContext(opts)
{
    public DbSet<Note> Notes => Set<Note>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Note>()
            .Property(n => n.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Note>()
            .Property(n => n.PublicId)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");
    }
}
