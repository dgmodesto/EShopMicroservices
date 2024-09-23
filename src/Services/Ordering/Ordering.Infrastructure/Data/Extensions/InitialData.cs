namespace Ordering.Infrastructure.Data.Extensions;

internal class InitialData
{
    public static IEnumerable<Customer> Customers =>
        new List<Customer>
        {
            Customer.Create(CustomerId.Of(new Guid("8fab5196-0860-4cf7-9d72-307ccdfd61a5")), "Customer 1", "customer1@gmail.com"),
            Customer.Create(CustomerId.Of(new Guid("625b6c2d-2c26-4f08-9f3d-d0aad4038af4")), "Custome 2", "customer2@gmail.com")
        };


    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(ProductId.Of(new Guid("4cf65d89-d3e5-4b4c-ac4e-ee031fc95f9f")), "IPhone X", 500),
            Product.Create(ProductId.Of(new Guid("df5a4d8e-a174-45bd-b3e0-cab5981abd14")), "Samsumb 10", 400),
            Product.Create(ProductId.Of(new Guid("cb910e45-f1f5-4522-92ab-a8df5f696858")), "Hauwei Plus", 650),
            Product.Create(ProductId.Of(new Guid("55e78b9d-f8c9-4215-b2ea-c08fcbdb50d3")), "Xiomi Mi", 450),
        };


    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address1 = Address.Of("douglas", "modesto", "d.modesto@gmail.com", "Av. Paulista, 1600", "Brazil", "Sao Paulo", "17200987");
            var address2 = Address.Of("john", "doe", "john@gmail.com", "Av. Consolação, 1430", "Brazil", "Sao Paulo", "17200987");

            var payment1 = Payment.Of("douglas", "5555555555554444", "12/29", "355", 1);
            var payment2 = Payment.Of("john", "8885555555554444", "06/30", "222", 1);

            var order1 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("8fab5196-0860-4cf7-9d72-307ccdfd61a5")),
                OrderName.Of("ORD_1"),
                shippingAddress: address1,
                billingAddress: address1,
                payment1);

            order1.Add(ProductId.Of(new Guid("4cf65d89-d3e5-4b4c-ac4e-ee031fc95f9f")), 2, 500);
            order1.Add(ProductId.Of(new Guid("df5a4d8e-a174-45bd-b3e0-cab5981abd14")), 1, 400);


            var order2 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("625b6c2d-2c26-4f08-9f3d-d0aad4038af4")),
                OrderName.Of("ORD_2"),
                shippingAddress: address2,
                billingAddress: address2,
                payment2);

            order2.Add(ProductId.Of(new Guid("cb910e45-f1f5-4522-92ab-a8df5f696858")), 3, 650);
            order2.Add(ProductId.Of(new Guid("55e78b9d-f8c9-4215-b2ea-c08fcbdb50d3")), 2, 450);

            return new List<Order>
            {
                order1,
                order2
            };

        }
    }

}