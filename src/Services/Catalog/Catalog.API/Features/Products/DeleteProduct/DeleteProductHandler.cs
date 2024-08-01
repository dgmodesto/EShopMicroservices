
namespace Catalog.API.Features.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
    }
}
internal class DeleteProductHandler(IDocumentSession session, ILogger<DeleteProductHandler> logger) 
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id);
        if (product is null) {
            throw new ProductNotFoundExceptions(command.Id);
        }

        session.Delete(product);
        await session.SaveChangesAsync();

        return new DeleteProductResult(true);
    }
}
