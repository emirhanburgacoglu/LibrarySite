namespace LibrarySite.Core.Domain
{
    public enum ReservationStatus
    {
        Pending = 0,
        Approved = 1,
        Cancelled = 2
    }

    public class Reservation
    {
        public int ReservationId { get; set; }
        public int BookId { get; set; }
        public int MemberUserId { get; set; }
        public ReservationStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
