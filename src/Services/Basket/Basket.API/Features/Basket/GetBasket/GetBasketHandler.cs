namespace Basket.API.Features.Basket.GetBasket;

public record GetBasketQuery(string UserName) 
    : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);

public class GetBasketHandler
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        //Get basket from database
        // var basket = await _repository.GetBasket(query.UserName);

        var cart = new ShoppingCart("dgmodesto");
        return await Task.FromResult(new GetBasketResult(cart));
    }
}
