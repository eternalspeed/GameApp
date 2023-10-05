using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GameApp.Models;

public partial class GameReviewDbContext : DbContext
{
    public GameReviewDbContext()
    {
    }

    public GameReviewDbContext(DbContextOptions<GameReviewDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Artwork> Artworks { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Award> Awards { get; set; }

    public virtual DbSet<Cast> Casts { get; set; }

    public virtual DbSet<CastRole> CastRoles { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Series> Series { get; set; }

    public virtual DbSet<Studio> Studios { get; set; }

    public virtual DbSet<StudioRole> StudioRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=GameReviewDB;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artwork>(entity =>
        {
            entity.ToTable("Artwork");

            entity.Property(e => e.ArtworkId).HasColumnName("ArtworkID");
            entity.Property(e => e.ArtworkTitle).HasMaxLength(50);
            entity.Property(e => e.ArtworkUrl)
                .HasMaxLength(50)
                .HasColumnName("ArtworkURL");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.Artworks)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_Artwork_Game");

            entity.HasOne(d => d.User).WithMany(p => p.Artworks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Artwork_AspNetUsers");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Award>(entity =>
        {
            entity.ToTable("Award");

            entity.Property(e => e.AwardId).HasColumnName("AwardID");
            entity.Property(e => e.AwardName).HasMaxLength(50);
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.Awards)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_Award_Game");

            entity.HasOne(d => d.User).WithMany(p => p.Awards)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Award_AspNetUsers");
        });

        modelBuilder.Entity<Cast>(entity =>
        {
            entity.ToTable("Cast");

            entity.Property(e => e.CastId).HasColumnName("CastID");
            entity.Property(e => e.CastRoleId).HasColumnName("CastRoleID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Sex)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Surname).HasMaxLength(50);

            entity.HasOne(d => d.CastRole).WithMany(p => p.Casts)
                .HasForeignKey(d => d.CastRoleId)
                .HasConstraintName("FK_Cast_CastRole");

            entity.HasOne(d => d.Game).WithMany(p => p.Casts)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_Cast_Game");
        });

        modelBuilder.Entity<CastRole>(entity =>
        {
            entity.ToTable("CastRole");

            entity.Property(e => e.CastRoleId).HasColumnName("CastRoleID");
            entity.Property(e => e.CastRoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("Game");

            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Engine).HasMaxLength(50);
            entity.Property(e => e.GameTitle).HasMaxLength(50);
            entity.Property(e => e.Genere).HasMaxLength(50);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(50)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Mode).HasMaxLength(50);
            entity.Property(e => e.Platform).HasMaxLength(50);
            entity.Property(e => e.ReleaseDate).HasColumnType("date");
            entity.Property(e => e.Score).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.SeriesId).HasColumnName("SeriesID");
            entity.Property(e => e.StudioId).HasColumnName("StudioID");

            entity.HasOne(d => d.Series).WithMany(p => p.Games)
                .HasForeignKey(d => d.SeriesId)
                .HasConstraintName("FK_Game_Series");

            entity.HasOne(d => d.Studio).WithMany(p => p.Games)
                .HasForeignKey(d => d.StudioId)
                .HasConstraintName("FK_Game_Studio");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Review");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.Body).HasMaxLength(500);
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.ReviewTitle).HasMaxLength(50);
            entity.Property(e => e.Score).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_Review_Game");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Review_AspNetUsers");
        });

        modelBuilder.Entity<Series>(entity =>
        {
            entity.Property(e => e.SeriesId).HasColumnName("SeriesID");
            entity.Property(e => e.SeriesName).HasMaxLength(50);
        });

        modelBuilder.Entity<Studio>(entity =>
        {
            entity.ToTable("Studio");

            entity.Property(e => e.StudioId).HasColumnName("StudioID");
            entity.Property(e => e.StudioName).HasMaxLength(50);
            entity.Property(e => e.StudioRoleId).HasColumnName("StudioRoleID");
            entity.Property(e => e.Website).HasMaxLength(50);

            entity.HasOne(d => d.StudioRole).WithMany(p => p.Studios)
                .HasForeignKey(d => d.StudioRoleId)
                .HasConstraintName("FK_Studio_StudioRole");
        });

        modelBuilder.Entity<StudioRole>(entity =>
        {
            entity.ToTable("StudioRole");

            entity.Property(e => e.StudioRoleId).HasColumnName("StudioRoleID");
            entity.Property(e => e.StudioRoleName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
