namespace Webshop.Service.Products;

public interface IProductService
{
    Product? GetProduct(string id);
    int GetProductInventory(string productId);
    void SubtractFromProductInventory(string productId, int quantity);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Product? GetProduct(string id)
    {
        return _productRepository.GetProduct(id);
    }

    public int GetProductInventory(string productId)
    {
        return _productRepository.GetProductInventory(productId);
    }

    public void SubtractFromProductInventory(string productId, int quantity)
    {
        var existingInventory = _productRepository.GetProductInventory(productId);
        var newInventory = existingInventory - quantity;
        _productRepository.UpdateProductInventory(productId, newInventory);
    }
}