package com.example.prm_ecommerce.API.Repository;

import com.example.prm_ecommerce.API.APIClient;
import com.example.prm_ecommerce.API.Interface.IOrderDetailService;
import com.example.prm_ecommerce.API.Interface.IOrderService;

public class OrderRepository {
    public static IOrderService getService() {
        return APIClient.getClient().create(IOrderService.class);
    }
}
