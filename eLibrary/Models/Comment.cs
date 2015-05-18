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
        /// <summary>
        /// Id комментария
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        [Required]
        [Display(Name = "Текст комментария")]
        public string Text { get; set; }

        /// <summary>
        /// Время комментария
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Книга с комментарием
        /// </summary>
        [Required]
        [Display(Name = "К книге")]
        public int BookId { get; set; }

        /// <summary>
        /// Книга с комментарием
        /// </summary>
        public Book Book { get; set; }

        /// <summary>
        /// Пользователеь
        /// </summary>
        [Required]
        [Display(Name = "Пользователь")]
        public int UserId { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public User User { get; set; }

    }
}