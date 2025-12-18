using LibrarySite.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySite.Core.Services
{
    public class BookService
    {
        private readonly List<Book> _books = new()
        {
            new Book { BookId = 1, Title = "Clean Code", Author = "Robert C. Martin", IsAvailable = true },
            new Book { BookId = 2, Title = "Design Patterns", Author = "GoF", IsAvailable = true },
            new Book { BookId = 3, Title = "Introduction to Algorithms", Author = "CLRS", IsAvailable = true }
        };

        public List<Book> GetAll()
        {
            return _books;
        }

        public Book? GetById(int id)
        {
            return _books.FirstOrDefault(b => b.BookId == id);
        }
    }
}
