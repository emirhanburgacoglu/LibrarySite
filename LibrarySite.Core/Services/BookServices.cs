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
        public void MarkAsUnavailable(int bookId)
{
    var book = _books.FirstOrDefault(b => b.BookId == bookId);
    if (book != null)
        book.IsAvailable = false;
}
public void MarkAsAvailable(int bookId)
{
    var book = _books.FirstOrDefault(b => b.BookId == bookId);
    if (book != null)
        book.IsAvailable = true;
}


        public Book? GetById(int id)
        {
            return _books.FirstOrDefault(b => b.BookId == id);
        }
        public (bool ok, string message) AddBook(string title, string author)
{
    title = (title ?? "").Trim();
    author = (author ?? "").Trim();

    if (string.IsNullOrWhiteSpace(title))
        return (false, "Title is required.");

    if (string.IsNullOrWhiteSpace(author))
        return (false, "Author is required.");

    // Eğer _books static list ise, id üretimi için max+1 yapıyoruz
    var newId = _books.Any() ? _books.Max(b => b.BookId) + 1 : 1;

    _books.Add(new Book
    {
        BookId = newId,
        Title = title,
        Author = author,
        IsAvailable = true
    });

    return (true, "Book added successfully.");
}

public (bool ok, string message) UpdateBook(int bookId, string title, string author, bool isAvailable)
{
    title = (title ?? "").Trim();
    author = (author ?? "").Trim();

    if (string.IsNullOrWhiteSpace(title))
        return (false, "Title is required.");

    if (string.IsNullOrWhiteSpace(author))
        return (false, "Author is required.");

    var book = _books.FirstOrDefault(b => b.BookId == bookId);
    if (book == null)
        return (false, "Book not found.");

    book.Title = title;
    book.Author = author;
    book.IsAvailable = isAvailable;

    return (true, "Book updated successfully.");
}
public (bool ok, string message) DeleteBook(int bookId)
{
    var book = _books.FirstOrDefault(b => b.BookId == bookId);
    if (book == null)
        return (false, "Book not found.");

    _books.Remove(book);
    return (true, "Book deleted successfully.");
}




    }
}
