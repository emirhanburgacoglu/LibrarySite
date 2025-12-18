using LibrarySite.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySite.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminReservationsController : Controller
    {
        private readonly ReservationService _reservationService;

        public AdminReservationsController()
        {
            _reservationService = new ReservationService();
        }

        public IActionResult Index()
        {
            var pending = _reservationService.GetPendingReservations();
            return View(pending);
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var (ok, message) = _reservationService.ApproveReservation(id);
            TempData["AdminResMessage"] = message;
            TempData["AdminResOk"] = ok;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var (ok, message) = _reservationService.CancelReservationByAdmin(id);
            TempData["AdminResMessage"] = message;
            TempData["AdminResOk"] = ok;
            return RedirectToAction("Index");
        }
    }
}
