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
    }
}
