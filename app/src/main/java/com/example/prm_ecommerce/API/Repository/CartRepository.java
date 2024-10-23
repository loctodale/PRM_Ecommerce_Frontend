package com.example.prm_ecommerce.API.Repository;

import com.example.prm_ecommerce.API.APIClient;
import com.example.prm_ecommerce.API.Interface.ICartService;

public class CartRepository {
    public static ICartService getCartService() {
        return APIClient.getClient().create(ICartService.class);
    }
}
