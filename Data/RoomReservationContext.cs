using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RoomReservation.Models;

#nullable disable

namespace RoomReservation.Data
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

        public virtual DbSet<TCategory> TCategories { get; set; }
        public virtual DbSet<TImg> TImgs { get; set; }
        public virtual DbSet<TRate> TRates { get; set; }
        public virtual DbSet<TReservation> TReservations { get; set; }
        public virtual DbSet<TReservationRoom> TReservationRooms { get; set; }
        public virtual DbSet<TRoom> TRooms { get; set; }
        public virtual DbSet<TSurvey> TSurveys { get; set; }
        public virtual DbSet<TUser> TUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TCategory>(entity =>
            {
                entity.Property(e => e.CatId).IsUnicode(false);

                entity.Property(e => e.CatType).IsUnicode(false);
            });

            modelBuilder.Entity<TImg>(entity =>
            {
                entity.Property(e => e.CatId).IsUnicode(false);

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.TImgs)
                    .HasForeignKey(d => d.CatId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_t_img_t_category");
            });

            modelBuilder.Entity<TRate>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoomId });

                entity.Property(e => e.Rate).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.TRates)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_rate_t_room");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TRates)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_rate_t_user");
            });

            modelBuilder.Entity<TReservation>(entity =>
            {
                entity.Property(e => e.ResDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TReservations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_t_reservation_t_user");
            });

            modelBuilder.Entity<TReservationRoom>(entity =>
            {
                entity.HasKey(e => new { e.ResId, e.RoomId });

                entity.HasOne(d => d.Res)
                    .WithMany(p => p.TReservationRooms)
                    .HasForeignKey(d => d.ResId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_reservation_room_t_reservation");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.TReservationRooms)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_reservation_room_t_room");
            });

            modelBuilder.Entity<TRoom>(entity =>
            {
                entity.Property(e => e.CatId).IsUnicode(false);

                entity.Property(e => e.RoomStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.TRooms)
                    .HasForeignKey(d => d.CatId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_t_room_t_category");
            });

            modelBuilder.Entity<TSurvey>(entity =>
            {
                entity.HasOne(d => d.Res)
                    .WithMany(p => p.TSurveys)
                    .HasForeignKey(d => d.ResId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_survey_t_reservation");
            });

            modelBuilder.Entity<TUser>(entity =>
            {
                entity.Property(e => e.UserAddress).IsUnicode(false);

                entity.Property(e => e.UserEmail).IsFixedLength(true);

                entity.Property(e => e.UserName).IsUnicode(false);

                entity.Property(e => e.UserPass)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UserStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserType).HasDefaultValueSql("((1))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
