using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InternAppWebAPI.Models
{
    public partial class InternAppDBContext : DbContext
    {
        public InternAppDBContext()
        {
        }

        public InternAppDBContext(DbContextOptions<InternAppDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RosenUser> RosenUsers { get; set; } = null!;
        public virtual DbSet<Title> Titles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-ERU7966;Database=InternAppDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RosenUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("RosenUser");

                entity.Property(e => e.UserCompany)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserDob).HasColumnType("date");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UserFirstName).HasMaxLength(80);

                entity.Property(e => e.UserGender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserImage)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UserLastName).HasMaxLength(80);

                entity.HasOne(d => d.UserTitle)
                    .WithMany(p => p.RosenUsers)
                    .HasForeignKey(d => d.UserTitleId)
                    .HasConstraintName("FK_RosenUser_Title");
            });

            modelBuilder.Entity<Title>(entity =>
            {
                entity.ToTable("Title");

                entity.Property(e => e.TitleName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
