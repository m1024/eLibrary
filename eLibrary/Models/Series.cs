using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class Series
    {
        /// <summary>
        /// Id серии
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Серия
        /// </summary>
        [Required]
        [Display(Name = "Серия")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}