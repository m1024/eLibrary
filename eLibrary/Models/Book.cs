using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class Book
    {
        // ID 
        public int Id { get; set; }

        // Имя
        [Required]
        [Display(Name = "Имя")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }

        public float Rating { get; set; }

        public int Votes { get; set; }

        public int Downloads { get; set; }
        [Display(Name = "Год создания")]
        public int ImprintYear { get; set; }


        [Display(Name = "Аннотация")]
        public string Annotation { get; set; }


        [Display(Name = "Текст_fb2")]
        public byte[] Text_fb2 { get; set; }

        [Display(Name = "Текст_epub")]
        public byte[] Text_epub { get; set; }

        [Display(Name = "Текст_txt")]
        public byte[] Text_txt { get; set; }

        [Display(Name = "Серия")]
        public int SerieId { get; set; }
        public Series Serie { get; set; }

        [Display(Name = "Жанр")]
        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        [Display(Name = "Обложка")]
        public byte[] Image { get; set; }


        [Display(Name = "Автор")]
        public virtual ICollection<Author> Authors { get; set; }

        public Book()
        {
            Authors=new List<Author>();
        }

    }
}