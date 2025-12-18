using System.Collections.Generic;
using System.Linq;
using LibrarySite.Core.Domain;

namespace LibrarySite.Core.Services
{
    public class ReservationService
    {
        // In-memory rezervasyonlar (DB gelince burası değişecek)
        private static readonly List<Reservation> _reservations = new();
        private static int _nextId = 1;

        public (bool ok, string message) CreateBookReservation(int memberUserId, int bookId)
        {
            // İş kuralı: Aynı kitap için Pending/Approved aktif rezervasyon varken yenisini açma
            var hasActive = _reservations.Any(r =>
                r.BookId == bookId &&
                (r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Approved));

            if (hasActive)
                return (false, "This book already has an active reservation.");

            var res = new Reservation
            {
                ReservationId = _nextId++,
                MemberUserId = memberUserId,
                BookId = bookId,
                Status = ReservationStatus.Pending
            };

            _reservations.Add(res);
            return (true, "Reservation created (Pending).");
        }

        public List<Reservation> GetAll()
        {
            return _reservations.ToList();
        }

          public List<Reservation> GetPendingReservations()
        {
            return _reservations
                .Where(r => r.Status == ReservationStatus.Pending)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        public (bool ok, string message) ApproveReservation(int reservationId)
        {
            var res = _reservations.FirstOrDefault(r => r.ReservationId == reservationId);
            if (res == null) return (false, "Reservation not found.");

            if (res.Status != ReservationStatus.Pending)
                return (false, "Only pending reservations can be approved.");

            res.Status = ReservationStatus.Approved;
            return (true, "Reservation approved.");
        }

        public (bool ok, string message) CancelReservationByAdmin(int reservationId)
        {
            var res = _reservations.FirstOrDefault(r => r.ReservationId == reservationId);
            if (res == null) return (false, "Reservation not found.");

            if (res.Status == ReservationStatus.Cancelled)
                return (false, "Reservation is already cancelled.");

            res.Status = ReservationStatus.Cancelled;
            return (true, "Reservation cancelled by admin.");
        }
    }

}
