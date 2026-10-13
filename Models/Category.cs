using System.ComponentModel.DataAnnotations;

namespace Categories.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Въведете име на категория.")]
    public string Name { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
