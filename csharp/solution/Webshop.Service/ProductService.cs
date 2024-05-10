namespace Webshop.Service
{
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

    public interface IProductRepository
    {
        Product? GetProduct(string id);
        int GetProductInventory(string productId);
        void UpdateProductInventory(string productId, int newInventory);
    }

    public class ProductRepository : IProductRepository
    {
        private Dictionary<string, Product> _productDatabase = new Dictionary<string, Product>();
        private Dictionary<string, int> _inventoryDatabase = new Dictionary<string, int>();

        public Product? GetProduct(string id)
        {
            if (_productDatabase.TryGetValue(id, out var product))
                return product;
            return null;
        }

        public int GetProductInventory(string productId)
        {
            if (_inventoryDatabase.TryGetValue(productId, out var productInventory))
                return productInventory;
            return 0;
        }

        public void UpdateProductInventory(string productId, int newInventory)
        {
            _inventoryDatabase[productId] = newInventory;
        }
    }

    public class Product
    {
        public string Id { get; set; }
        public decimal Price { get; set; }
    }
}
