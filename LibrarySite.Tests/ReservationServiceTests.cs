using LibrarySite.Core.Services;
using LibrarySite.Core.Domain;
using Xunit;

namespace LibrarySite.Tests;

public class ReservationServiceTests
{
    [Fact]
    public void CreateReservation_WhenResourceAvailable_ShouldSucceed()
    {
        var svc = new ReservationService();
        svc.ResetForTests();

        var (ok, message) = svc.CreateBookReservation(memberUserId: 1, bookId: 10);

        Assert.True(ok);
        Assert.Contains("Pending", message);
        Assert.Single(svc.GetAll());
        Assert.Equal(ReservationStatus.Pending, svc.GetAll()[0].Status);
    }

    [Fact]
    public void CreateReservation_WhenConflictExists_ShouldFail()
    {
        var svc = new ReservationService();
        svc.ResetForTests();

        var first = svc.CreateBookReservation(memberUserId: 1, bookId: 10);
        var second = svc.CreateBookReservation(memberUserId: 2, bookId: 10);

        Assert.True(first.ok);
        Assert.False(second.ok);
        Assert.Contains("active reservation", second.message);
        Assert.Single(svc.GetAll());
    }

    [Fact]
    public void CancelPendingReservation_ShouldSucceed()
    {
        var svc = new ReservationService();
        svc.ResetForTests();

        svc.CreateBookReservation(memberUserId: 1, bookId: 20);
        var resId = svc.GetAll().Single().ReservationId;

        var (ok, message) = svc.CancelReservationByAdmin(resId);

        Assert.True(ok);
        Assert.Contains("cancelled", message.ToLower());

        var updated = svc.GetAll().Single();
        Assert.Equal(ReservationStatus.Cancelled, updated.Status);
    }
}
