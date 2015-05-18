using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class Book
    {
        /// <summary>
        /// Id книги
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование книги
        /// </summary>
        [Required]
        [Display(Name = "Наименование")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }

        /// <summary>
        /// Рейтинг книги
        /// </summary>
        public float Rating { get; set; }

        /// <summary>
        /// Число голосов
        /// </summary>
        public int Votes { get; set; }

        /// <summary>
        /// Число загрузок
        /// </summary>
        public int Downloads { get; set; }

        /// <summary>
        /// Год создания
        /// </summary>
        [Display(Name = "Год создания")]
        public int ImprintYear { get; set; }

        /// <summary>
        /// Аннотация
        /// </summary>
        [Display(Name = "Аннотация")]
        public string Annotation { get; set; }

        /// <summary>
        /// Книга в fb2
        /// </summary>
        [Display(Name = "Текст_fb2")]
        public byte[] Text_fb2 { get; set; }

        /// <summary>
        /// Книга в epub
        /// </summary>
        [Display(Name = "Текст_epub")]
        public byte[] Text_epub { get; set; }

        /// <summary>
        /// Книга в txt
        /// </summary>
        [Display(Name = "Текст_txt")]
        public byte[] Text_txt { get; set; }

        /// <summary>
        /// Серия
        /// </summary>
        [Display(Name = "Серия")]
        public int SerieId { get; set; }

        /// <summary>
        /// Серия
        /// </summary>
        public Series Serie { get; set; }

        /// <summary>
        /// Жанр
        /// </summary>
        [Display(Name = "Жанр")]
        public int GenreId { get; set; }

        /// <summary>
        /// Жанр
        /// </summary>
        public Genre Genre { get; set; }

        /// <summary>
        /// Обложка
        /// </summary>
        [Display(Name = "Обложка")]
        public byte[] Image { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        [Display(Name = "Автор")]
        public virtual ICollection<Author> Authors { get; set; }

        /// <summary>
        /// Связь с авторами, один ко многим
        /// </summary>
        public Book()
        {
            Authors = new List<Author>();
        }

    }
}