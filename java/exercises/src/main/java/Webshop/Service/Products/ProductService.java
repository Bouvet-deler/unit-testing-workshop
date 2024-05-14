package Webshop.Service.Products;

interface IProductService {
    Product getProduct(String id);
    int getProductInventory(String productId);
    void subtractFromProductInventory(String productId, int quantity);
}

public class ProductService implements IProductService {
    private final IProductRepository productRepository;

    public ProductService(IProductRepository productRepository) {
        this.productRepository = productRepository;
    }

    @Override
    public Product getProduct(String id) {
        return productRepository.getProduct(id);
    }

    @Override
    public int getProductInventory(String productId) {
        return productRepository.getProductInventory(productId);
    }

    @Override
    public void subtractFromProductInventory(String productId, int quantity) {
        int existingInventory = productRepository.getProductInventory(productId);
        int newInventory = existingInventory - quantity;
        productRepository.updateProductInventory(productId, newInventory);
    }
}

