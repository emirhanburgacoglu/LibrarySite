using System.Security.Claims;
using LibrarySite.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySite.Web.Controllers
{
    [Authorize(Roles = "Member,Admin")] // şimdilik giriş yapan herkes reserve denemesin, biz kontrol edeceğiz
    public class ReservationsController : Controller
    {
        private readonly ReservationService _reservationService;

        public ReservationsController()
        {
            _reservationService = new ReservationService();
        }

        [Authorize(Roles = "Member")] // Asıl kural: rezervasyon sadece member
        [HttpPost]
        public IActionResult ReserveBook(int bookId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdStr))
                return RedirectToAction("Login", "Auth");

            var memberUserId = int.Parse(userIdStr);

            var (ok, message) = _reservationService.CreateBookReservation(memberUserId, bookId);

            TempData["ReservationMessage"] = message;
            TempData["ReservationOk"] = ok;

            return RedirectToAction("Index", "Books");
        }
        [Authorize(Roles = "Member")]
public IActionResult MyBookReservations()
{
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var memberUserId))
        return RedirectToAction("Login", "Auth");

    var list = _reservationService.GetReservationsByMember(memberUserId);
    return View(list);
}

    }
}
