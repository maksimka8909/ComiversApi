using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Models
{
    [Table("User")]
    [Microsoft.EntityFrameworkCore.Index(nameof(Login), Name = "Login", IsUnique = true)]
    public partial class User
    {
        public User()
        {
            ComicsReadByUsers = new HashSet<ComicsReadByUser>();
            ComicsScores = new HashSet<ComicsScore>();
            Comments = new HashSet<Comment>();
            IssueReadByUsers = new HashSet<IssueReadByUser>();
            TrackedComics = new HashSet<TrackedComic>();
            UserBookmarks = new HashSet<UserBookmark>();
            UserFavouriteGenres = new HashSet<UserFavouriteGenre>();
        }

        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(40)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Login { get; set; } = null!;
        [StringLength(255)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Password { get; set; } = null!;
        [StringLength(40)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Name { get; set; } = null!;
        [StringLength(10000)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Photo { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime LastLog { get; set; }
        [StringLength(100)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Email { get; set; } = null!;
        [Required]
        public bool? Access { get; set; }
        [Required]
        public bool? Role { get; set; }

        [InverseProperty(nameof(ComicsReadByUser.IdUserNavigation))]
        public virtual ICollection<ComicsReadByUser> ComicsReadByUsers { get; set; }
        [InverseProperty(nameof(ComicsScore.IdUserNavigation))]
        public virtual ICollection<ComicsScore> ComicsScores { get; set; }
        [InverseProperty(nameof(Comment.IdUserNavigation))]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty(nameof(IssueReadByUser.IdUserNavigation))]
        public virtual ICollection<IssueReadByUser> IssueReadByUsers { get; set; }
        [InverseProperty(nameof(TrackedComic.IdUserNavigation))]
        public virtual ICollection<TrackedComic> TrackedComics { get; set; }
        [InverseProperty(nameof(UserBookmark.IdUserNavigation))]
        public virtual ICollection<UserBookmark> UserBookmarks { get; set; }
        [InverseProperty(nameof(UserFavouriteGenre.IdUserNavigation))]
        public virtual ICollection<UserFavouriteGenre> UserFavouriteGenres { get; set; }
    }
}
