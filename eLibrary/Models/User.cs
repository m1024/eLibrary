using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLibrary.Models
{
    public class User
    {
        // ID 
        public int Id { get; set; }

        // Логин
        [Required]
        [Display(Name = "Логин")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Login { get; set; }

        // Пароль
        [Required]
        [Display(Name = "Пароль")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Password { get; set; }

        // Фамилия Имя Отчество
        [Display(Name = "Фамилия Имя Отчество")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }

        // Информация
        [Display(Name = "Информация")]
        public string Information { get; set; }

        // Роль
        [Required]
        [Display(Name = "Статус")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

    }
}