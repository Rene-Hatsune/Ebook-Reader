using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookList.Models;

namespace BookList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksContext _context;

        public BooksController(BooksContext context)
        {
            _context = context;

            if(_context.BookItems.Count() == 0)
            {
                _context.BookItems.Add(new BookItem {Title = "JavaScript Cookbook", Genre="Technology" });
                _context.BookItems.Add(new BookItem { Title = "Haskell Cookbook", Genre = "Technology" });
                _context.SaveChanges();
            }
        }
        
        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookItem>>> GetBookItems()
        {
            return await _context.BookItems.ToListAsync();
        }

        // GET: api/Books/2
        [HttpGet("{id}")]
        public async Task<ActionResult<BookItem>> GetBookItem(int id)
        {
            var bookItem = await _context.BookItems.FindAsync(id);

            if (bookItem == null)
            {
                return NotFound();
            }

            return bookItem;
        }

        [HttpPost]
        public async Task<ActionResult<BookItem>> PostBookItem(BookItem book)
        {
            _context.BookItems.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBookItem), new { id = book.BookItemId }, book);
        }

        // PUT: api/Books/2
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFileItem(int id, BookItem book)
        {
            if (id != book.BookItemId)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Books/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookItem(int id)
        {
            var bookItem = await _context.BookItems.FindAsync(id);

            if (bookItem == null)
            {
                return NotFound();
            }

            _context.BookItems.Remove(bookItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}