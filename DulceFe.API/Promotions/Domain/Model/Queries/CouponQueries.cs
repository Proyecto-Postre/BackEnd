namespace DulceFe.API.Promotions.Domain.Model.Queries;

public record GetAllCouponsQuery();
public record GetCouponByIdQuery(int Id);
public record GetCouponByCodeQuery(string Code);
