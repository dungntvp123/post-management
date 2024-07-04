using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRN_Project.Models;

public partial class FinalProjectContext : DbContext
{
    public FinalProjectContext()
    {
    }

    public FinalProjectContext(DbContextOptions<FinalProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Emojy> Emojies { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5861A9FD5D3");

            entity.HasIndex(e => e.Credential, "UQ__Accounts__B5DADE8AC724E47F").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Credential)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFAAA44EEE1D");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Comments__PostID__30F848ED");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Comments__UserID__300424B4");
        });

        modelBuilder.Entity<Emojy>(entity =>
        {
            entity.HasKey(e => e.EmojiId).HasName("PK__Emojies__D584B2139D5E0EB2");

            entity.Property(e => e.EmojiId).HasColumnName("EmojiID");
            entity.Property(e => e.Content)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.EmoteDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Post).WithMany(p => p.Emojies)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Emojies__PostID__35BCFE0A");

            entity.HasOne(d => d.User).WithMany(p => p.Emojies)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Emojies__UserID__34C8D9D1");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Posts__AA126038E18841BF");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.PostDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(1000);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Posts__UserID__2C3393D0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC74FE29F2");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.NickName)
                .HasMaxLength(64)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Users)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Users__AccountID__286302EC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
