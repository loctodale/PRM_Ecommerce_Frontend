package com.example.prm_ecommerce.API.Repository;

import com.example.prm_ecommerce.API.APIClient;
import com.example.prm_ecommerce.API.Interface.IProductService;

public class ProductRepository {
    public static IProductService getProductService() {
        return APIClient.getClient().create(IProductService.class);
    }
}
