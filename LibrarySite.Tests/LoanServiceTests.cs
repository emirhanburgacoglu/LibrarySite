using LibrarySite.Core.Domain;
using LibrarySite.Core.Services;
using Xunit;

namespace LibrarySite.Tests;

public class LoanServiceTests
{
    [Fact]
    public void BorrowBook_WhenAvailable_ShouldSucceed()
    {
        var svc = new LoanService();
        svc.ResetForTests();

        var (ok, msg) = svc.BorrowBook(memberUserId: 1, bookId: 10);

        Assert.True(ok);
        Assert.Contains("borrowed", msg.ToLower());
        Assert.Single(svc.GetAll());
        Assert.Equal(LoanStatus.Borrowed, svc.GetAll()[0].Status);
    }

    [Fact]
    public void BorrowBook_WhenAlreadyBorrowed_ShouldFail()
    {
        var svc = new LoanService();
        svc.ResetForTests();

        var first = svc.BorrowBook(memberUserId: 1, bookId: 10);
        var second = svc.BorrowBook(memberUserId: 2, bookId: 10);

        Assert.True(first.ok);
        Assert.False(second.ok);
        Assert.Contains("already borrowed", second.message.ToLower());
        Assert.Single(svc.GetAll());
    }

    [Fact]
    public void ReturnBook_ByOwner_ShouldSucceed()
    {
        var svc = new LoanService();
        svc.ResetForTests();

        svc.BorrowBook(memberUserId: 7, bookId: 55);
        var loan = svc.GetAll().Single();

        var (ok, msg) = svc.ReturnBook(loan.LoanId, memberUserId: 7);

        Assert.True(ok);
        Assert.Contains("returned", msg.ToLower());
        Assert.Equal(LoanStatus.Returned, loan.Status);
        Assert.NotNull(loan.ReturnedAt);
    }

    [Fact]
    public void ReturnBook_ByAnotherUser_ShouldFail()
    {
        var svc = new LoanService();
        svc.ResetForTests();

        svc.BorrowBook(memberUserId: 7, bookId: 55);
        var loan = svc.GetAll().Single();

        var result = svc.ReturnBook(loan.LoanId, memberUserId: 99);

        Assert.False(result.ok);
        Assert.Contains("unauthorized", result.message.ToLower());
        Assert.Equal(LoanStatus.Borrowed, loan.Status);
        Assert.Null(loan.ReturnedAt);
    }
}
