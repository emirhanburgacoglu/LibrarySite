using LibrarySite.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySite.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminStudyRoomReservationsController : Controller
    {
        private readonly StudyRoomReservationService _service;

        public AdminStudyRoomReservationsController(StudyRoomReservationService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var pending = _service.GetPendingReservations();
            return View(pending);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            var (ok, msg) = _service.ApproveReservation(id);
            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            var (ok, msg) = _service.CancelReservationByAdmin(id);
            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction(nameof(Index));
        }
    }
}
