package Webshop.Service.Products;

import java.util.HashMap;
import java.util.Map;

public class ProductRepository implements IProductRepository {
    private final Map<String, Product> productDatabase = new HashMap<>();
    private final Map<String, Integer> inventoryDatabase = new HashMap<>();

    @Override
    public Product getProduct(String id) {
        return productDatabase.get(id);
    }

    @Override
    public int getProductInventory(String productId) {
        return inventoryDatabase.getOrDefault(productId, 0);
    }

    @Override
    public void updateProductInventory(String productId, int newInventory) {
        inventoryDatabase.put(productId, newInventory);
    }
}

interface IProductRepository {
    Product getProduct(String id);
    int getProductInventory(String productId);
    void updateProductInventory(String productId, int newInventory);
}
