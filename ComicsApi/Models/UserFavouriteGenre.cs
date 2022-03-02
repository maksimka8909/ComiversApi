using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Models
{
    [Microsoft.EntityFrameworkCore.Index(nameof(IdGenre), Name = "ID_Genre")]
    [Microsoft.EntityFrameworkCore.Index(nameof(IdUser), Name = "ID_User")]
    public partial class UserFavouriteGenre
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column("ID_User", TypeName = "int(11)")]
        public int? IdUser { get; set; }
        [Column("ID_Genre", TypeName = "int(11)")]
        public int? IdGenre { get; set; }

        [ForeignKey(nameof(IdGenre))]
        [InverseProperty(nameof(Genre.UserFavouriteGenres))]
        public virtual Genre? IdGenreNavigation { get; set; }
        [ForeignKey(nameof(IdUser))]
        [InverseProperty(nameof(User.UserFavouriteGenres))]
        public virtual User? IdUserNavigation { get; set; }
    }
}
