﻿using Microsoft.Extensions.Logging;

namespace Catalog.API.Features.Products.GetProductById;

public record GetProductByIdQuery(Guid Id): IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

internal class GetProductByIdHandler(
    IDocumentSession session, ILogger<GetProductByIdHandler> logger)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundExceptions(query.Id);
        }

        return new GetProductByIdResult(product);
    }
}
