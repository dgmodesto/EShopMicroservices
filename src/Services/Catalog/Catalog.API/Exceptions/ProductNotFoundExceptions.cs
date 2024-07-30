namespace Catalog.API.Exceptions;

public class ProductNotFoundExceptions : Exception
{
    public ProductNotFoundExceptions() 
        : base("Product not found!")
    {
            
    }
}
