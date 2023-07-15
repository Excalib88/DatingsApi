using System.ComponentModel.DataAnnotations;
using Datings.Api.Data.Entities;

namespace Datings.Api.Models.Identity;

public class RegisterRequest
{
    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;
    
    [Display(Name = "Phone")] 
    public string Phone { get; set; } = null!;
 
    [Required]
    [Display(Name = "Дата рождения")]
    public DateTime? BirthDate { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;

    [Required]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string PasswordConfirm { get; set; } = null!;

    [Required]
    [Display(Name = "Имя")]
    public string FirstName { get; set; } = null!;
    
    public Gender? Gender { get; set; }
    public string? FindNow { get; set; }
    public List<string> Interests { get; set; } = new();
}