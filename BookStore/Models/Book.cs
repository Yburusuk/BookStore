using System.ComponentModel.DataAnnotations;

namespace BookStore.Models;

public class Book
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string? Name { get; set; }
    [Required]
    [MaxLength(50)]
    public string? Author { get; set; }
    [Required]
    [Range(0,200)]
    public decimal Price { get; set; }
}