using Xunit;
using DulceFe.API.Promotions.Domain.Model.Aggregates;

namespace DulceFe.API.Tests.Promotions;

public class CouponTests
{
    [Fact]
    public void Coupon_Should_Be_Active_By_Default()
    {
        var coupon = new Coupon { Code = "TEST10", DiscountValue = 10 };
        Assert.True(coupon.IsActive);
    }

    [Fact]
    public void Coupon_Should_Allow_Percentage_Discount()
    {
        var coupon = new Coupon { Code = "PERCENT", DiscountValue = 15, IsPercentage = true };
        Assert.True(coupon.IsPercentage);
        Assert.Equal(15, coupon.DiscountValue);
    }
}
