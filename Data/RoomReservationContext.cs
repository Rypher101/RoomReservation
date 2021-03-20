using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RoomReservation.Models
{
    public partial class RoomReservationContext : DbContext
    {
        public RoomReservationContext()
        {
        }

        public RoomReservationContext(DbContextOptions<RoomReservationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TCategory> TCategory { get; set; }
        public virtual DbSet<TCategoryImage> TCategoryImage { get; set; }
        public virtual DbSet<TImg> TImg { get; set; }
        public virtual DbSet<TRate> TRate { get; set; }
        public virtual DbSet<TReservation> TReservation { get; set; }
        public virtual DbSet<TReservationRoom> TReservationRoom { get; set; }
        public virtual DbSet<TRoom> TRoom { get; set; }
        public virtual DbSet<TUser> TUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TCategory>(entity =>
            {
                entity.HasIndex(e => e.CatType)
                    .HasName("IX_t_category")
                    .IsUnique();

                entity.Property(e => e.CatId).IsUnicode(false);

                entity.Property(e => e.CatType).IsUnicode(false);
            });

            modelBuilder.Entity<TCategoryImage>(entity =>
            {
                entity.HasKey(e => new { e.CatId, e.ImgId });

                entity.Property(e => e.CatId).IsUnicode(false);

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.TCategoryImage)
                    .HasForeignKey(d => d.CatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_category_image_t_category");

                entity.HasOne(d => d.Img)
                    .WithMany(p => p.TCategoryImage)
                    .HasForeignKey(d => d.ImgId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_category_image_t_img");
            });

            modelBuilder.Entity<TRate>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoomId });

                entity.Property(e => e.Rate).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.TRate)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_rate_t_room");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TRate)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_rate_t_user");
            });

            modelBuilder.Entity<TReservation>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.TReservation)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_t_reservation_t_user");
            });

            modelBuilder.Entity<TReservationRoom>(entity =>
            {
                entity.HasKey(e => new { e.ResId, e.RoomId });

                entity.Property(e => e.ResId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Res)
                    .WithMany(p => p.TReservationRoom)
                    .HasForeignKey(d => d.ResId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_reservation_room_t_reservation");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.TReservationRoom)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_reservation_room_t_room");
            });

            modelBuilder.Entity<TRoom>(entity =>
            {
                entity.Property(e => e.CatId).IsUnicode(false);

                entity.Property(e => e.RoomStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.TRoom)
                    .HasForeignKey(d => d.CatId)
                    .HasConstraintName("FK_t_room_t_category");
            });

            modelBuilder.Entity<TUser>(entity =>
            {
                entity.HasIndex(e => e.UserEmail)
                    .HasName("IX_t_user")
                    .IsUnique();

                entity.Property(e => e.UserAddress).IsUnicode(false);

                entity.Property(e => e.UserEmail).IsFixedLength();

                entity.Property(e => e.UserName).IsUnicode(false);

                entity.Property(e => e.UserPass)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
