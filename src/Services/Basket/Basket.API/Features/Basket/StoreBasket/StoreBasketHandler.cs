namespace Basket.API.Features.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) 
    : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(r => r.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(r => r.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class StoreBasketHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart cart = command.Cart;

        //TODO: store basket in database (use marten upsert - if exist = update, it not exist = insert
        //TODO: update cache

        return new StoreBasketResult("dgmodesto");
    }
}
