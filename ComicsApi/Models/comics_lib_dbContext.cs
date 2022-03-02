using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ComicsApi.Models
{
    public partial class comics_lib_dbContext : DbContext
    {
        public comics_lib_dbContext()
        {
        }

        public comics_lib_dbContext(DbContextOptions<comics_lib_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Comic> Comics { get; set; } = null!;
        public virtual DbSet<ComicsReadByUser> ComicsReadByUsers { get; set; } = null!;
        public virtual DbSet<ComicsScore> ComicsScores { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Editor> Editors { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Issue> Issues { get; set; } = null!;
        public virtual DbSet<IssueReadByUser> IssueReadByUsers { get; set; } = null!;
        public virtual DbSet<ListOfComicsGenre> ListOfComicsGenres { get; set; } = null!;
        public virtual DbSet<ListOfIssue> ListOfIssues { get; set; } = null!;
        public virtual DbSet<TrackedComic> TrackedComics { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserBookmark> UserBookmarks { get; set; } = null!;
        public virtual DbSet<UserFavouriteGenre> UserFavouriteGenres { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=194.32.248.98;user id=maxk;password=Maxk123!;persistsecurityinfo=True;database=comics_lib_db", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.36-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("latin1_swedish_ci")
                .HasCharSet("latin1");

            modelBuilder.Entity<Comic>(entity =>
            {
                entity.HasOne(d => d.IdAuthorNavigation)
                    .WithMany(p => p.Comics)
                    .HasForeignKey(d => d.IdAuthor)
                    .HasConstraintName("Comics_ibfk_1");

                entity.HasOne(d => d.IdEditorNavigation)
                    .WithMany(p => p.Comics)
                    .HasForeignKey(d => d.IdEditor)
                    .HasConstraintName("Comics_ibfk_2");
            });

            modelBuilder.Entity<ComicsReadByUser>(entity =>
            {
                entity.HasOne(d => d.IdComicsNavigation)
                    .WithMany(p => p.ComicsReadByUsers)
                    .HasForeignKey(d => d.IdComics)
                    .HasConstraintName("ComicsReadByUser_ibfk_1");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.ComicsReadByUsers)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("ComicsReadByUser_ibfk_2");
            });

            modelBuilder.Entity<ComicsScore>(entity =>
            {
                entity.HasOne(d => d.IdComicsNavigation)
                    .WithMany(p => p.ComicsScores)
                    .HasForeignKey(d => d.IdComics)
                    .HasConstraintName("ComicsScore_ibfk_1");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.ComicsScores)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("ComicsScore_ibfk_2");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.IdComicsNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdComics)
                    .HasConstraintName("Comments_ibfk_1");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("Comments_ibfk_2");
            });

            modelBuilder.Entity<IssueReadByUser>(entity =>
            {
                entity.HasOne(d => d.IdIssueNavigation)
                    .WithMany(p => p.IssueReadByUsers)
                    .HasForeignKey(d => d.IdIssue)
                    .HasConstraintName("IssueReadByUser_ibfk_1");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.IssueReadByUsers)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("IssueReadByUser_ibfk_2");
            });

            modelBuilder.Entity<ListOfComicsGenre>(entity =>
            {
                entity.HasOne(d => d.IdComicsNavigation)
                    .WithMany(p => p.ListOfComicsGenres)
                    .HasForeignKey(d => d.IdComics)
                    .HasConstraintName("ListOfComicsGenres_ibfk_1");

                entity.HasOne(d => d.IdGenreNavigation)
                    .WithMany(p => p.ListOfComicsGenres)
                    .HasForeignKey(d => d.IdGenre)
                    .HasConstraintName("ListOfComicsGenres_ibfk_2");
            });

            modelBuilder.Entity<ListOfIssue>(entity =>
            {
                entity.HasOne(d => d.IdComicsNavigation)
                    .WithMany(p => p.ListOfIssues)
                    .HasForeignKey(d => d.IdComics)
                    .HasConstraintName("ListOfIssues_ibfk_1");

                entity.HasOne(d => d.IdIssueNavigation)
                    .WithMany(p => p.ListOfIssues)
                    .HasForeignKey(d => d.IdIssue)
                    .HasConstraintName("ListOfIssues_ibfk_2");
            });

            modelBuilder.Entity<TrackedComic>(entity =>
            {
                entity.HasOne(d => d.IdComicsNavigation)
                    .WithMany(p => p.TrackedComics)
                    .HasForeignKey(d => d.IdComics)
                    .HasConstraintName("TrackedComics_ibfk_1");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.TrackedComics)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("TrackedComics_ibfk_2");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Access).HasDefaultValueSql("'1'");

                entity.Property(e => e.Role).HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<UserBookmark>(entity =>
            {
                entity.HasOne(d => d.IdIssueNavigation)
                    .WithMany(p => p.UserBookmarks)
                    .HasForeignKey(d => d.IdIssue)
                    .HasConstraintName("UserBookmarks_ibfk_2");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UserBookmarks)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("UserBookmarks_ibfk_1");
            });

            modelBuilder.Entity<UserFavouriteGenre>(entity =>
            {
                entity.HasOne(d => d.IdGenreNavigation)
                    .WithMany(p => p.UserFavouriteGenres)
                    .HasForeignKey(d => d.IdGenre)
                    .HasConstraintName("UserFavouriteGenres_ibfk_2");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UserFavouriteGenres)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("UserFavouriteGenres_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
