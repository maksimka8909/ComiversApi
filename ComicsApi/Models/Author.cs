using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Models
{
    [Table("Author")]
    public partial class Author
    {
        public Author()
        {
            Comics = new HashSet<Comic>();
        }

        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(40)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Surname { get; set; } = null!;
        [StringLength(40)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Name { get; set; } = null!;
        [StringLength(40)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string? MiddleName { get; set; }
        public DateTime Birthday { get; set; }
        [StringLength(10000)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Photo { get; set; } = null!;
        [StringLength(1024)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Description { get; set; } = null!;

        [InverseProperty(nameof(Comic.IdAuthorNavigation))]
        public virtual ICollection<Comic> Comics { get; set; }
    }
}
