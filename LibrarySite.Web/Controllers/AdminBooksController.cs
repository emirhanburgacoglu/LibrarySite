using LibrarySite.Core.Services;
using LibrarySite.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySite.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBooksController : Controller
    {
        private readonly BookService _bookService;

        public AdminBooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            var books = _bookService.GetAll();
            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AdminBookCreateVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AdminBookCreateVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var (ok, message) = _bookService.AddBook(vm.Title, vm.Author);

            if (!ok)
            {
                ModelState.AddModelError("", message);
                return View(vm);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
public IActionResult Edit(int id)
{
    var book = _bookService.GetById(id);
    if (book == null) return NotFound();

    var vm = new AdminBookEditVm
    {
        BookId = book.BookId,
        Title = book.Title,
        Author = book.Author,
        IsAvailable = book.IsAvailable
    };

    return View(vm);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Edit(AdminBookEditVm vm)
{
    if (!ModelState.IsValid)
        return View(vm);

    var (ok, message) = _bookService.UpdateBook(vm.BookId, vm.Title, vm.Author, vm.IsAvailable);

    if (!ok)
    {
        ModelState.AddModelError("", message);
        return View(vm);
    }

    TempData["Success"] = message;
    return RedirectToAction(nameof(Index));
}

    }
}
