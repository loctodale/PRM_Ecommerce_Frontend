package com.example.prm_ecommerce.API.Repository;

import com.example.prm_ecommerce.API.APIClient;
import com.example.prm_ecommerce.API.Interface.IDeliveryService;
import com.example.prm_ecommerce.API.Interface.IOrderDetailService;

public class DeliveryRepository {
    public static IDeliveryService getService() {
        return APIClient.getClient().create(IDeliveryService.class);
    }
}
