using System.Collections.Generic;
using System.Linq;
using LibrarySite.Core.Domain;

namespace LibrarySite.Core.Services
{
    public class LoanService
    {

        public void ResetForTests()
{
    _loans.Clear();
    _nextId = 1;
}

        // In-memory loan listesi (DB gelince değişecek)
        private static readonly List<Loan> _loans = new();
        private static int _nextId = 1;

        // SADECE ÖDÜNÇ ALMA (Borrow)
        public (bool ok, string message) BorrowBook(int memberUserId, int bookId)
        {
            // İş kuralı:
            // Aynı kitap başka bir kullanıcıda Borrowed durumundaysa
            // tekrar ödünç verilemez
            var alreadyBorrowed = _loans.Any(l =>
                l.BookId == bookId &&
                l.Status == LoanStatus.Borrowed);

            if (alreadyBorrowed)
                return (false, "Book is already borrowed.");

            var loan = new Loan
            {
                LoanId = _nextId++,
                MemberUserId = memberUserId,
                BookId = bookId,
                Status = LoanStatus.Borrowed
            };

            _loans.Add(loan);
            return (true, "Book borrowed successfully.");
        }
        
        // Kitap iade etme (Return)
public (bool ok, string message) ReturnBook(int loanId, int memberUserId)
{
    var loan = _loans.FirstOrDefault(l => l.LoanId == loanId);

    if (loan == null)
        return (false, "Loan not found.");

    // Sadece kitabı ödünç alan kişi iade edebilir
    if (loan.MemberUserId != memberUserId)
        return (false, "Unauthorized return attempt.");

    if (loan.Status == LoanStatus.Returned)
        return (false, "Book already returned.");

    loan.Status = LoanStatus.Returned;
    loan.ReturnedAt = DateTime.UtcNow;

    return (true, "Book returned successfully.");
}

        // Şimdilik sadece kontrol amaçlı
        public List<Loan> GetAll()
        {
            return _loans.ToList();
        }
    }
}
