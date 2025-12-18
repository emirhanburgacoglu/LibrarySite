using System;

namespace LibrarySite.Core.Domain
{
    public class StudyRoomReservation
    {
        public int ReservationId { get; set; }

        public int StudyRoomId { get; set; }
        public int MemberUserId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ReservationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
