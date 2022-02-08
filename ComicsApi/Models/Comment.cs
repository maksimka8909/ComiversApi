using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Models
{
    [Microsoft.EntityFrameworkCore.Index(nameof(IdComics), Name = "ID_Comics")]
    [Microsoft.EntityFrameworkCore.Index(nameof(IdUser), Name = "ID_User")]
    public partial class Comment
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(255)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Body { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        [Column("ID_Comics", TypeName = "int(11)")]
        public int? IdComics { get; set; }
        [Column("ID_User", TypeName = "int(11)")]
        public int? IdUser { get; set; }

        [ForeignKey(nameof(IdComics))]
        [InverseProperty(nameof(Comic.Comments))]
        public virtual Comic? IdComicsNavigation { get; set; }
        [ForeignKey(nameof(IdUser))]
        [InverseProperty(nameof(User.Comments))]
        public virtual User? IdUserNavigation { get; set; }
    }
}
