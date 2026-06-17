using System;
using System.ComponentModel.DataAnnotations;

namespace Elegant.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        // Зберігається лише хеш паролю (PBKDF2, System.Web.Helpers.Crypto)
        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // Чи є цей користувач адміністратором (тільки elegant@gmail.com)
        public bool IsAdmin { get; set; }
    }
}
