namespace LibrarySite.Core.Domain
{
    // Kitabın ödünç durumunu temsil eder
    public enum LoanStatus
    {
        Borrowed = 0,
        Returned = 1
    }

    // Bir kitabın bir kullanıcıya ödünç verilmesini temsil eder
    public class Loan
    {
        public int LoanId { get; set; }

        public int BookId { get; set; }

        public int MemberUserId { get; set; }

        public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReturnedAt { get; set; }

        public LoanStatus Status { get; set; } = LoanStatus.Borrowed;
    }
}
