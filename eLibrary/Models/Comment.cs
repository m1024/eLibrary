using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;

namespace eLibrary.Models
{
    public class Comment
    {
        // ID 
        public int Id { get; set; }

        [Required]
        [Display(Name = "Текст комментария")]
        public string Text { get; set; }


        public DateTime Time { get; set; }


        [Required]
        [Display(Name = "К книге")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Required]
        [Display(Name = "Пользователь")]
        public int UserId { get; set; }
        public User User { get; set; }

    }
}