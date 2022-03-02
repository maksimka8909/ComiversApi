using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Models
{
    [Table("ComicsScore")]
    [Microsoft.EntityFrameworkCore.Index(nameof(IdComics), Name = "ID_Comics")]
    [Microsoft.EntityFrameworkCore.Index(nameof(IdUser), Name = "ID_User")]
    public partial class ComicsScore
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column(TypeName = "int(11)")]
        public int Mark { get; set; }
        [Column("ID_Comics", TypeName = "int(11)")]
        public int? IdComics { get; set; }
        [Column("ID_User", TypeName = "int(11)")]
        public int? IdUser { get; set; }
        [Column(TypeName = "int(11)")]
        public int NumberOfRated { get; set; }

        [ForeignKey(nameof(IdComics))]
        [InverseProperty(nameof(Comic.ComicsScores))]
        public virtual Comic? IdComicsNavigation { get; set; }
        [ForeignKey(nameof(IdUser))]
        [InverseProperty(nameof(User.ComicsScores))]
        public virtual User? IdUserNavigation { get; set; }
    }
}
