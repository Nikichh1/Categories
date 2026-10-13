using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Categories.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Въведете име на продукт.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Категория")]
    public int IdCategory { get; set; }

    [ForeignKey(nameof(IdCategory))]
    public Category? Category { get; set; }
}
