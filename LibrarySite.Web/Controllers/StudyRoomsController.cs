using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibrarySite.Core.Services;
using LibrarySite.Web.ViewModels;

namespace LibrarySite.Web.Controllers
{
    [Authorize(Roles = "Member")]
    public class StudyRoomsController : Controller
    {
        private readonly StudyRoomReservationService _service;

        public StudyRoomsController(StudyRoomReservationService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var rooms = _service.GetAllRooms();
            return View(rooms);
        }

        [HttpGet]
        public IActionResult Reserve(int id)
        {
            var room = _service.GetRoomById(id);
            if (room == null) return NotFound();

            ViewBag.RoomName = room.Name;

            var vm = new StudyRoomReserveVm
            {
                StudyRoomId = id,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2)
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reserve(StudyRoomReserveVm vm)
        {
            var room = _service.GetRoomById(vm.StudyRoomId);
            ViewBag.RoomName = room?.Name ?? "Study Room";

            if (!ModelState.IsValid)
                return View(vm);

            // ✅ UserId = NameIdentifier
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var memberUserId))
            {
                ModelState.AddModelError("", "UserId not found in claims. Please login again.");
                return View(vm);
            }

            var (ok, message) = _service.CreateStudyRoomReservation(
                memberUserId,
                vm.StudyRoomId,
                vm.StartTime,
                vm.EndTime);

            if (!ok)
            {
                ModelState.AddModelError("", message);
                return View(vm);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(MyReservations));
        }

        public IActionResult MyReservations()
        {
            // ✅ UserId = NameIdentifier
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var memberUserId))
                return RedirectToAction("Index", "Books");

            var list = _service.GetReservationsByMember(memberUserId);
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            // ✅ UserId = NameIdentifier
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var memberUserId))
                return RedirectToAction(nameof(MyReservations));

            var (ok, message) = _service.CancelByMember(id, memberUserId);
            TempData[ok ? "Success" : "Error"] = message;

            return RedirectToAction(nameof(MyReservations));
        }
    }
}
