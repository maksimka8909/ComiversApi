using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Models
{
    [Microsoft.EntityFrameworkCore.Index(nameof(IdComics), Name = "ID_Comics")]
    [Microsoft.EntityFrameworkCore.Index(nameof(IdGenre), Name = "ID_Genre")]
    public partial class ListOfComicsGenre
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column("ID_Comics", TypeName = "int(11)")]
        public int? IdComics { get; set; }
        [Column("ID_Genre", TypeName = "int(11)")]
        public int? IdGenre { get; set; }

        [ForeignKey(nameof(IdComics))]
        [InverseProperty(nameof(Comic.ListOfComicsGenres))]
        public virtual Comic? IdComicsNavigation { get; set; }
        [ForeignKey(nameof(IdGenre))]
        [InverseProperty(nameof(Genre.ListOfComicsGenres))]
        public virtual Genre? IdGenreNavigation { get; set; }
    }
}
