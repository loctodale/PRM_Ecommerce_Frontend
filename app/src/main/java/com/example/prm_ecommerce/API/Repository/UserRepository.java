package com.example.prm_ecommerce.API.Repository;

import com.example.prm_ecommerce.API.APIClient;
import com.example.prm_ecommerce.API.Interface.IProductService;
import com.example.prm_ecommerce.API.Interface.IUserService;

public class UserRepository {
    public static IUserService getUserService() {
        return APIClient.getClient().create(IUserService.class);
    }
}
