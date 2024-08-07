namespace Basket.API.Features.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccess);
public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(r => r.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class DeleteBasketHandler(IBasketRepository respository)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        await respository.DeleteBasket(command.UserName, cancellationToken);
        return new DeleteBasketResult(true);
    }
}
