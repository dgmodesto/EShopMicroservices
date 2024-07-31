using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions;

public class ProductNotFoundExceptions : NotFoundException
{
    public ProductNotFoundExceptions(Guid Id) 
        : base("Product", Id)
    {
            
    }
}
