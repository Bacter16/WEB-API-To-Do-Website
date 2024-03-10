using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(32)]
        public string Username { get; set; }

        [Required]
        [StringLength(32)]
        public string Email { get; set; }

        [Required]
        [StringLength(32)]
        public byte[] PasswordHash { get; set; }

        [Required]
        [StringLength(16)]
        public byte[] PasswordSalt { get; set; }
    }
}
