using Discount.Grpc;
using Discount.Grpc.Protos;
using Grpc.Core;

namespace Discount.Grpc.Services;
public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{ 
    private readonly ILogger<DiscountService> _logger;
    public DiscountService(ILogger<DiscountService> logger)
    {
        _logger = logger;
    }

    public override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        return base.GetDiscount(request, context);
    }

    public override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        return base.CreateDiscount(request, context);
    }

    public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        return base.UpdateDiscount(request, context);
    }

    public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        return base.DeleteDiscount(request, context);
    }
}   
