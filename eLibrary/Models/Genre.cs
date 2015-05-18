using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class Genre
    {
        /// <summary>
        /// Id жанр
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Жанр
        /// </summary>
        [Required]
        [Display(Name = "Жанр")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }
    }
}