using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Library_project.Models;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Categorybook> Categorybooks { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Recordbook> Recordbooks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;database=library;uid=root;password=211103", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("book");

            entity.HasIndex(e => e.Category, "fk_Book_CategoryBook_idx");

            entity.HasIndex(e => e.Manufacturer, "fk_Book_Manufacturer1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Avtor).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Books)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Book_CategoryBook");

            entity.HasOne(d => d.ManufacturerNavigation).WithMany(p => p.Books)
                .HasForeignKey(d => d.Manufacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Book_Manufacturer1");
        });

        modelBuilder.Entity<Categorybook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categorybook");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("manufacturer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<Recordbook>(entity =>
        {
            entity.HasKey(e => new { e.UsersId, e.BookId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("recordbook");

            entity.HasIndex(e => e.BookId, "fk_Users_has_Book_Book1_idx");

            entity.HasIndex(e => e.UsersId, "fk_Users_has_Book_Users1_idx");

            entity.Property(e => e.UsersId).HasColumnName("Users_id");
            entity.Property(e => e.BookId).HasColumnName("Book_id");
            entity.Property(e => e.DataTimeReturn).HasColumnType("datetime");
            entity.Property(e => e.DataTimeTake).HasColumnType("datetime");

            entity.HasOne(d => d.Book).WithMany(p => p.Recordbooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Users_has_Book_Book1");

            entity.HasOne(d => d.Users).WithMany(p => p.Recordbooks)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Users_has_Book_Users1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasMaxLength(45);
            entity.Property(e => e.Login).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Password).HasMaxLength(45);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
