using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Models
{
    [Table("IssueReadByUser")]
    [Microsoft.EntityFrameworkCore.Index(nameof(IdIssue), Name = "ID_Issue")]
    [Microsoft.EntityFrameworkCore.Index(nameof(IdUser), Name = "ID_User")]
    public partial class IssueReadByUser
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column("ID_Issue", TypeName = "int(11)")]
        public int? IdIssue { get; set; }
        [Column("ID_User", TypeName = "int(11)")]
        public int? IdUser { get; set; }

        [ForeignKey(nameof(IdIssue))]
        [InverseProperty(nameof(Issue.IssueReadByUsers))]
        public virtual Issue? IdIssueNavigation { get; set; }
        [ForeignKey(nameof(IdUser))]
        [InverseProperty(nameof(User.IssueReadByUsers))]
        public virtual User? IdUserNavigation { get; set; }
    }
}
