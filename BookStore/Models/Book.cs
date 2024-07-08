namespace BookStore.Models;

public class Book
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Author { get; set; }
    public required decimal Price { get; set; }
}