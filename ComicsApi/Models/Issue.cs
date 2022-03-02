using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Models
{
    [Table("Issue")]
    public partial class Issue
    {
        public Issue()
        {
            IssueReadByUsers = new HashSet<IssueReadByUser>();
            ListOfIssues = new HashSet<ListOfIssue>();
            UserBookmarks = new HashSet<UserBookmark>();
        }

        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(255)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string NameIssue { get; set; } = null!;
        [StringLength(255)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string NameFile { get; set; } = null!;
        [StringLength(255)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string PathRead { get; set; } = null!;
        [StringLength(255)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string PathDownload { get; set; } = null!;
        public DateOnly DateOfPublication { get; set; }

        [InverseProperty(nameof(IssueReadByUser.IdIssueNavigation))]
        public virtual ICollection<IssueReadByUser> IssueReadByUsers { get; set; }
        [InverseProperty(nameof(ListOfIssue.IdIssueNavigation))]
        public virtual ICollection<ListOfIssue> ListOfIssues { get; set; }
        [InverseProperty(nameof(UserBookmark.IdIssueNavigation))]
        public virtual ICollection<UserBookmark> UserBookmarks { get; set; }
    }
}
