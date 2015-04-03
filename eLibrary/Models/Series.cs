using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class Series
    {
        // ID 
        public int Id { get; set; }


        [Required]
        [Display(Name = "Серия")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}