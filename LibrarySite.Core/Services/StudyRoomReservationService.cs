using System;
using System.Collections.Generic;
using System.Linq;
using LibrarySite.Core.Domain;

namespace LibrarySite.Core.Services
{
    public class StudyRoomReservationService
    {
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




        // In-memory odalar ve rezervasyonlar (DB gelince değişecek)
        private static readonly List<StudyRoom> _rooms = new()
        {
            new StudyRoom { StudyRoomId = 1, Name = "Room A", Capacity = 4 },
            new StudyRoom { StudyRoomId = 2, Name = "Room B", Capacity = 6 },
            new StudyRoom { StudyRoomId = 3, Name = "Room C", Capacity = 2 }
        };

        private static readonly List<StudyRoomReservation> _reservations = new();
        private static int _nextId = 1;

        // Odaları listele
        public List<StudyRoom> GetAllRooms()
        {
            return _rooms.ToList();
        }

        public StudyRoom? GetRoomById(int roomId)
        {
            return _rooms.FirstOrDefault(r => r.StudyRoomId == roomId);
        }

        // Member'ın rezervasyonları (My Reservations ekranı için)
        public List<StudyRoomReservation> GetReservationsByMember(int memberUserId)
        {
            return _reservations
                .Where(r => r.MemberUserId == memberUserId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        // Admin ekranı için (istersen sonra kullanırız)
        public List<StudyRoomReservation> GetPendingReservations()
        {
            return _reservations
                .Where(r => r.Status == ReservationStatus.Pending)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        // ✅ Asıl iş: Study room rezervasyonu oluştur (çakışma önleme burada)
        public (bool ok, string message) CreateStudyRoomReservation(
            int memberUserId,
            int studyRoomId,
            DateTime startTime,
            DateTime endTime)
        {
            // 1) basit validasyon
            if (startTime >= endTime)
                return (false, "Start time must be earlier than end time.");

            // İstersen bunu DateTime.UtcNow ile de yapabilirsin ama UI genelde local saatle gelir
            if (startTime < DateTime.Now)
                return (false, "You cannot create a reservation in the past.");

            // 2) oda var mı?
            var roomExists = _rooms.Any(r => r.StudyRoomId == studyRoomId);
            if (!roomExists)
                return (false, "Study room not found.");

            // 3) çakışma kontrolü (Pending/Approved aktif sayılır, Cancelled sayılmaz)
            var hasConflict = _reservations.Any(r =>
                r.StudyRoomId == studyRoomId &&
                (r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Approved) &&
                IntervalsOverlap(startTime, endTime, r.StartTime, r.EndTime)
            );

            if (hasConflict)
                return (false, "This room is already reserved for the selected time range.");

            // 4) rezervasyonu oluştur
            var res = new StudyRoomReservation
            {
                ReservationId = _nextId++,
                MemberUserId = memberUserId,
                StudyRoomId = studyRoomId,
                StartTime = startTime,
                EndTime = endTime,
                Status = ReservationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _reservations.Add(res);
            return (true, "Study room reservation created (Pending).");
        }

        // Member kendi rezervasyonunu iptal edebilir (My Reservations ekranı için)
        public (bool ok, string message) CancelByMember(int reservationId, int memberUserId)
        {
            var res = _reservations.FirstOrDefault(r => r.ReservationId == reservationId);
            if (res == null) return (false, "Reservation not found.");

            if (res.MemberUserId != memberUserId)
                return (false, "You cannot cancel someone else's reservation.");

            if (res.Status == ReservationStatus.Cancelled)
                return (false, "Reservation already cancelled.");

            res.Status = ReservationStatus.Cancelled;
            return (true, "Reservation cancelled.");
        }

        // Zaman aralığı kesişimi kontrolü:
        // A [start,end) ile B [start,end) kesişiyorsa true
        private static bool IntervalsOverlap(DateTime aStart, DateTime aEnd, DateTime bStart, DateTime bEnd)
        {
            return aStart < bEnd && aEnd > bStart;
        }
    }
}
