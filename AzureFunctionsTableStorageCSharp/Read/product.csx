using Microsoft.WindowsAzure.Storage.Table;

public class Product :TableEntity
{
    public string ProductName { get; set; }

    public double UnitPrice { get; set; }

    public int UnitsInStock { get; set; }

    public bool Discontinued { get; set; }

    public Product ToEntity()
    {
        return new Product
        {
            PartitionKey = "Product",
            RowKey = this?.RowKey,
            ProductName = this?.ProductName,
            UnitPrice = this.UnitPrice,
            UnitsInStock = this.UnitsInStock,
            Discontinued = this.Discontinued,
            ETag = "*"
        };
    }
}