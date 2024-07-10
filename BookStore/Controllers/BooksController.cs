using BookStore.Data;
using BookStore.Exceptions;
using BookStore.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DataContext _context;

        public BooksController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Book>>>> GetAllBooks()
        {
            var books = await _context.Books.ToListAsync();
            
            return new ApiResponse<List<Book>>(books);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Book>>> GetBookById(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Bad request.");
            
            var book = await _context.Books.FindAsync(id);
            
            if (book == null)
                throw new NotFoundException("Book not found.");
            
            return new ApiResponse<Book>(book);
        }

        [HttpGet("list")]
        public async Task<ActionResult<ApiResponse<List<Book>>>> ListBooks([FromQuery] string? name, [FromQuery] string? sortBy)
        {
            var books = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                books = books.Where(b => b.Name != null && b.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                books = sortBy.ToLower() switch
                {
                    "name" => books.OrderBy(b => b.Name),
                    "author" => books.OrderBy(b => b.Author),
                    "price" => books.OrderBy(b => b.Price),
                    _ => books
                };
            }

            var resultBooks = await books.ToListAsync();

            return new ApiResponse<List<Book>>(resultBooks);
        }
        
        [HttpPost]
        public async Task<ActionResult<ApiResponse<List<Book>>>> AddBook([FromBody] Book inBook)
        {
            _context.Books.Add(inBook);
            await _context.SaveChangesAsync();
            var allBooks = await _context.Books.ToListAsync();
            
            return new ApiResponse<List<Book>>(allBooks);
        }
        
        [HttpPut]
        public async Task<ActionResult<ApiResponse<List<Book>>>> UpdateBook([FromBody] Book inBook)
        {
            var book = await _context.Books.FindAsync(inBook.Id);
            if (book == null)
            {
                throw new NotFoundException("Book not found.");
            }

            book.Name = inBook.Name;
            book.Author = inBook.Author;
            book.Price = inBook.Price;

            await _context.SaveChangesAsync();

            var allBooks = await _context.Books.ToListAsync();

            return new ApiResponse<List<Book>>(allBooks);
        }
        
        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<List<Book>>>> PatchBook(int id, [FromBody] JsonPatchDocument<Book> updateBook)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                throw new NotFoundException("Book not found.");
            }

            var onBook = new Book {Id = book.Id, Name = book.Name, Author = book.Author, Price = book.Price};
            
            updateBook.ApplyTo(onBook, ModelState);

            if (!ModelState.IsValid)
            {
                throw new BadRequestException(ModelState);
            }

            book.Name = onBook.Name;
            book.Author = onBook.Author;
            book.Price = onBook.Price;

            await _context.SaveChangesAsync();

            var allBooks = await _context.Books.ToListAsync();
            
            return new ApiResponse<List<Book>>(allBooks);
        }
        
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<List<Book>>>> DeleteBook([FromQuery] int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                throw new NotFoundException("Book not found.");
            }

            _context.Books.Remove(book);
            
            await _context.SaveChangesAsync();

            var allBooks = await _context.Books.ToListAsync();
            
            return new ApiResponse<List<Book>>(allBooks);
        }
    }
}
