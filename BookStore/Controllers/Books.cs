using BookStore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            var books = await _context.Books.ToListAsync();
            
            return Ok(books);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound("Book not found.");
            }
            
            return Ok(book);
        }
        
        [HttpPost]
        public async Task<ActionResult<List<Book>>> AddBook(Book inBook)
        {
            _context.Books.Add(inBook);
            await _context.SaveChangesAsync();
            
            return Ok(await _context.Books.ToListAsync());
        }
        
        [HttpPut]
        public async Task<ActionResult<List<Book>>> UpdateBook(Book inBook)
        {
            var book = await _context.Books.FindAsync(inBook.Id);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            book.Name = inBook.Name;
            book.Author = inBook.Author;
            book.Price = inBook.Price;

            await _context.SaveChangesAsync();
            
            return Ok(await _context.Books.ToListAsync());
        }
        
        [HttpDelete]
        public async Task<ActionResult<List<Book>>> DeleteBook([FromBody] int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            _context.Books.Remove(book);
            
            await _context.SaveChangesAsync();
            
            return Ok(await _context.Books.ToListAsync());
        }
    }
}
