#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Crypto.Models;

namespace Crypto.Data
{
    public partial class crypto_web_app_dbContext : DbContext
    {
        public crypto_web_app_dbContext()
        {
        }

        public crypto_web_app_dbContext(DbContextOptions<crypto_web_app_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AlertHistory> AlertHistories { get; set; }
        public virtual DbSet<AlertPackage> AlertPackages { get; set; }
        public virtual DbSet<BlogPost> BlogPosts { get; set; }
        public virtual DbSet<BlogPostComment> BlogPostComments { get; set; }
        public virtual DbSet<BlogPostLike> BlogPostLikes { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Forum> Forums { get; set; }
	public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<ForumFeed> ForumFeeds { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<PriceTrack> PriceTracks { get; set; }
        public virtual DbSet<UserAlertPackage> UserAlertPackages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlertHistory>(entity =>
            {
                entity.ToTable("AlertHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BitUserId).HasColumnName("BitUserID");

                entity.Property(e => e.PackageName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("packageName");

                entity.Property(e => e.Price)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("price");

                entity.Property(e => e.SentAt)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BitUser)
                    .WithMany(p => p.AlertHistories)
                    .HasForeignKey(d => d.BitUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__AlertHist__BitUs__4316F928");
            });

            modelBuilder.Entity<AlertPackage>(entity =>
            {
                entity.ToTable("AlertPackage");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.PackageName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");
            });

            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.ToTable("BlogPost");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BitUserId).HasColumnName("BitUserID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.BitUser)
                    .WithMany(p => p.BlogPosts)
                    .HasForeignKey(d => d.BitUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__BlogPost__BitUse__2F10007B");
            });

            modelBuilder.Entity<BlogPostComment>(entity =>
            {
                entity.ToTable("BlogPostComment");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BitUserId).HasColumnName("BitUserID");

                entity.Property(e => e.BitUserName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BlogPostId).HasColumnName("BlogPostID");

                entity.Property(e => e.CommentText).IsUnicode(false);

                entity.Property(e => e.CommentedAt).HasColumnType("datetime");

                entity.HasOne(d => d.BitUser)
                    .WithMany(p => p.BlogPostComments)
                    .HasForeignKey(d => d.BitUserId)
                    .HasConstraintName("FK__BlogPostC__BitUs__32E0915F");

                entity.HasOne(d => d.BlogPost)
                    .WithMany(p => p.BlogPostComments)
                    .HasForeignKey(d => d.BlogPostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__BlogPostC__BlogP__31EC6D26");
            });

            modelBuilder.Entity<BlogPostLike>(entity =>
            {
                entity.ToTable("BlogPostLike");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BitUserId).HasColumnName("BitUserID");

                entity.Property(e => e.BitUserName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BlogPostId).HasColumnName("BlogPostID");

                entity.Property(e => e.LikedAt).HasColumnType("datetime");

                entity.HasOne(d => d.BitUser)
                    .WithMany(p => p.BlogPostLikes)
                    .HasForeignKey(d => d.BitUserId)
                    .HasConstraintName("FK__BlogPostL__BitUs__36B12243");

                entity.HasOne(d => d.BlogPost)
                    .WithMany(p => p.BlogPostLikes)
                    .HasForeignKey(d => d.BlogPostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__BlogPostL__BlogP__35BCFE0A");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.EventDate)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EventTime)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ImageLink).IsUnicode(false);

                entity.Property(e => e.Link).IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.PostedByNavigation)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.PostedBy)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Event__PostedBy__45F365D3");
            });

            modelBuilder.Entity<Forum>(entity =>
            {
                entity.ToTable("Forum");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BitUserId).HasColumnName("BitUserID");

                entity.Property(e => e.BitUserName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.BitUser)
                    .WithMany(p => p.Forums)
                    .HasForeignKey(d => d.BitUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Forum__BitUserID__276EDEB3");
            });

            modelBuilder.Entity<ForumFeed>(entity =>
            {
                entity.ToTable("ForumFeed");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BitUserId).HasColumnName("BitUserID");

                entity.Property(e => e.BitUserName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.FeedText).IsUnicode(false);

                entity.Property(e => e.ForumId).HasColumnName("ForumID");

                entity.HasOne(d => d.BitUser)
                    .WithMany(p => p.ForumFeeds)
                    .HasForeignKey(d => d.BitUserId)
                    .HasConstraintName("FK__ForumFeed__BitUs__2B3F6F97");

                entity.HasOne(d => d.Forum)
                    .WithMany(p => p.ForumFeeds)
                    .HasForeignKey(d => d.ForumId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ForumFeed__Forum__2A4B4B5E");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AboutMe).IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash).IsUnicode(false);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PriceTrack>(entity =>
            {
                entity.ToTable("PriceTrack");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BitUserId).HasColumnName("BitUserID");

                entity.Property(e => e.Price)
                    .IsUnicode(false)
                    .HasColumnName("price");

                entity.HasOne(d => d.BitUser)
                    .WithMany(p => p.PriceTracks)
                    .HasForeignKey(d => d.BitUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__PriceTrac__BitUs__403A8C7D");
            });

	    modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.Property(e => e.QuestionDescription)
                    .HasMaxLength(300)
                    .IsUnicode(false)
		    .HasColumnName("question");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.UId)
                    .HasConstraintName("FK_Question");
            });

            modelBuilder.Entity<UserAlertPackage>(entity =>
            {
                entity.ToTable("UserAlertPackage");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AlertPackageId).HasColumnName("AlertPackageID");

                entity.Property(e => e.BitUserId).HasColumnName("BitUserID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.AlertPackage)
                    .WithMany(p => p.UserAlertPackages)
                    .HasForeignKey(d => d.AlertPackageId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__UserAlert__Alert__3C69FB99");

                entity.HasOne(d => d.BitUser)
                    .WithMany(p => p.UserAlertPackages)
                    .HasForeignKey(d => d.BitUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__UserAlert__BitUs__3D5E1FD2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}