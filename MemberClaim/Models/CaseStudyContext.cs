using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MemberClaim.Models
{
    public partial class CaseStudyContext : DbContext
    {
        public CaseStudyContext()
        {
        }

        public CaseStudyContext(DbContextOptions<CaseStudyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Claim> Claims { get; set; } = null!;
        public virtual DbSet<Dependent> Dependents { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CaseStudy;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Claim>(entity =>
            {
                entity.ToTable("Claim");

                entity.Property(e => e.DateOfAdmission).HasColumnType("date");

                entity.Property(e => e.DateOfDischarge).HasColumnType("date");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.MemberName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProviderName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Claims)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK__Claim__MemberId__3C69FB99");
            });

            modelBuilder.Entity<Dependent>(entity =>
            {
                entity.ToTable("Dependent");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Dependents)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK__Dependent__Membe__38996AB5");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MemberId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasComputedColumnSql("('R-'+CONVERT([varchar](10),[Id]))", false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Pan)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("PAN");

                entity.Property(e => e.State)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Password)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
