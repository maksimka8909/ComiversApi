using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Models
{
    [Table("Editor")]
    [Microsoft.EntityFrameworkCore.Index(nameof(Name), Name = "Name", IsUnique = true)]
    public partial class Editor
    {
        public Editor()
        {
            Comics = new HashSet<Comic>();
        }

        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(40)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Name { get; set; } = null!;
        [StringLength(10000)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Photo { get; set; } = null!;

        [InverseProperty(nameof(Comic.IdEditorNavigation))]
        public virtual ICollection<Comic> Comics { get; set; }
    }
}
