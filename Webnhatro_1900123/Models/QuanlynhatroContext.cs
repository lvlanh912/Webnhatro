using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Webnhatro_1900123.Models
{
    public partial class QuanlynhatroContext : DbContext
    {
        public QuanlynhatroContext()
        {
        }

        public QuanlynhatroContext(DbContextOptions<QuanlynhatroContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<KhuVuc> KhuVucs { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<NhaTro> NhaTros { get; set; } = null!;
        public virtual DbSet<TinTuc> TinTucs { get; set; } = null!;
        public virtual DbSet<Tinh> Tinhs { get; set; } = null!;
        public virtual DbSet<Xa> Xas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-KISACQV\\SQLEXPRESS;Initial Catalog=Quanlynhatro;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Iduser);

                entity.ToTable("Admin");

                entity.Property(e => e.Iduser)
                    .HasMaxLength(10)
                    .HasColumnName("IDuser")
                    .IsFixedLength();

                entity.Property(e => e.HoTen).HasMaxLength(50);

                entity.Property(e => e.Pass)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<KhuVuc>(entity =>
            {
                entity.HasKey(e => e.IdkhuVuc)
                    .HasName("PK_TBlKhuVuc");

                entity.ToTable("KhuVuc");

                entity.Property(e => e.IdkhuVuc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IDKhuVuc");

                entity.Property(e => e.Idtinh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IDTinh");

                entity.Property(e => e.TenKhuVuc).HasMaxLength(50);

                entity.HasOne(d => d.IdtinhNavigation)
                    .WithMany(p => p.KhuVucs)
                    .HasForeignKey(d => d.Idtinh)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_TBlKhuVuc_tblTinh");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.ToTable("Member");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Diachi).HasMaxLength(250);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HoTen).HasMaxLength(250);

                entity.Property(e => e.Ngaythamgia).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<NhaTro>(entity =>
            {
                entity.HasKey(e => e.Idtro)
                    .HasName("PK_tblNhaTro");

                entity.ToTable("NhaTro");

                entity.Property(e => e.Idtro)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDTro");

                entity.Property(e => e.Anh)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Idxa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IDXa");

                entity.Property(e => e.Thoigiandang).HasColumnType("datetime");

                entity.Property(e => e.TieuDe).HasMaxLength(250);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdxaNavigation)
                    .WithMany(p => p.NhaTros)
                    .HasForeignKey(d => d.Idxa)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_NhaTro_Xa");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.NhaTros)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("FK_NhaTro_Member1");
            });

            modelBuilder.Entity<TinTuc>(entity =>
            {
                entity.HasKey(e => e.Idnews);

                entity.ToTable("TinTuc");

                entity.Property(e => e.Idnews)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDNews");

                entity.Property(e => e.Anh)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NgayDang).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(250);
            });

            modelBuilder.Entity<Tinh>(entity =>
            {
                entity.HasKey(e => e.Idtinh)
                    .HasName("PK_tblTinh");

                entity.ToTable("Tinh");

                entity.Property(e => e.Idtinh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IDTinh");

                entity.Property(e => e.TenTinh).HasMaxLength(50);
            });

            modelBuilder.Entity<Xa>(entity =>
            {
                entity.HasKey(e => e.Idxa);

                entity.ToTable("Xa");

                entity.Property(e => e.Idxa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IDXa");

                entity.Property(e => e.IdkhuVuc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IDKhuVuc");

                entity.Property(e => e.TenXa).HasMaxLength(50);

                entity.HasOne(d => d.IdkhuVucNavigation)
                    .WithMany(p => p.Xas)
                    .HasForeignKey(d => d.IdkhuVuc)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Xa_KhuVuc");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
