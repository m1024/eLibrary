using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class Role
    {
        /// <summary>
        /// Id роли
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [Required]
        [Display(Name = "Статус")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }

    }
}