using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class Author
    {
        // ID 
        public int Id { get; set; }

        // Имя
        [Display(Name = "Имя")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }

        //Фамилия
        [Required]
        [Display(Name = "Фамилия")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Family { get; set; }

        //Отчество
        [Display(Name = "Отчество")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Patronymic { get; set; }

        [Display(Name = "Фото")]
        public byte[] Image { get; set; }


        [Display(Name = "Информация")]
        public string Information { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}