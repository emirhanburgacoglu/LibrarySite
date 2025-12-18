using System.Security.Claims;
using LibrarySite.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySite.Web.Controllers
{
    [Authorize(Roles = "Member")]
    public class LoansController : Controller
    {
        private readonly LoanService _loanService;
        private readonly BookService _bookService;

        // ✅ SADECE TEK CONSTRUCTOR KALSIN
        public LoansController(LoanService loanService, BookService bookService)
        {
            _loanService = loanService;
            _bookService = bookService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Borrow(int bookId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var memberUserId))
            {
                TempData["Error"] = "User information not found. Please login again.";
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            var (ok, message) = _loanService.BorrowBook(memberUserId, bookId);

            if (ok)
            {
                // Kitabı UI tarafında da "not available" yapalım
                _bookService.MarkAsUnavailable(bookId);
            }

            TempData[ok ? "Success" : "Error"] = message;
            return RedirectToAction("Details", "Books", new { id = bookId });
        }
    }
}
