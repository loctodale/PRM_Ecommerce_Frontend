package com.example.prm_ecommerce.API.Repository;

import com.example.prm_ecommerce.API.APIClient;
import com.example.prm_ecommerce.API.Interface.IOrderDetailService;
import com.example.prm_ecommerce.API.Interface.IProductService;

public class OrderDetailRepository {
    public static IOrderDetailService getService() {
        return APIClient.getClient().create(IOrderDetailService.class);
    }
}
