package com.example.prm_ecommerce.API.Repository;

import com.example.prm_ecommerce.API.APIClient;
import com.example.prm_ecommerce.API.Interface.INotificationService;
import com.example.prm_ecommerce.API.Interface.IProductService;

public class NotificationRepository {
    public static INotificationService getNoticationService() {
        return APIClient.getClient().create(INotificationService.class);
    }
}
