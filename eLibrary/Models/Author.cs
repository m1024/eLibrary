using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class Author
    {
        /// <summary>
        /// Id автора
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя автора
        /// </summary>
        [Display(Name = "Имя")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }

        /// <summary>
        /// Фамилия автора
        /// </summary>
        [Required]
        [Display(Name = "Фамилия")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Family { get; set; }

        /// <summary>
        /// Отчество автора
        /// </summary>
        [Display(Name = "Отчество")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Patronymic { get; set; }

        /// <summary>
        /// Фото автора
        /// </summary>
        [Display(Name = "Фото")]
        public byte[] Image { get; set; }

        /// <summary>
        /// Информация об авторе
        /// </summary>
        [Display(Name = "Информация")]
        public string Information { get; set; }

        /// <summary>
        /// Книги
        /// </summary>
        public virtual ICollection<Book> Books { get; set; }
    }
}