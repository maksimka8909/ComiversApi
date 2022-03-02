using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Models
{
    [Microsoft.EntityFrameworkCore.Index(nameof(IdComics), Name = "ID_Comics")]
    [Microsoft.EntityFrameworkCore.Index(nameof(IdIssue), Name = "ID_Issue")]
    public partial class ListOfIssue
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column("ID_Comics", TypeName = "int(11)")]
        public int? IdComics { get; set; }
        [Column("ID_Issue", TypeName = "int(11)")]
        public int? IdIssue { get; set; }

        [ForeignKey(nameof(IdComics))]
        [InverseProperty(nameof(Comic.ListOfIssues))]
        public virtual Comic? IdComicsNavigation { get; set; }
        [ForeignKey(nameof(IdIssue))]
        [InverseProperty(nameof(Issue.ListOfIssues))]
        public virtual Issue? IdIssueNavigation { get; set; }
    }
}
