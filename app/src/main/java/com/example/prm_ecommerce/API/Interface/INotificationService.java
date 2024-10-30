package com.example.prm_ecommerce.API.Interface;

import com.example.prm_ecommerce.Model.RequestAddProductToCartModel;
import com.example.prm_ecommerce.domain.CartDomain;
import com.example.prm_ecommerce.domain.NotificationDomain;
import com.example.prm_ecommerce.domain.ProductDomain;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

public interface INotificationService {
    String NOTIFICATION = "notification";

    @GET(NOTIFICATION + "/getNotification/{userId}")
    Call<NotificationDomain[]> getNotificationByUserId(@Path("userId")Object userId);

    @GET(NOTIFICATION + "/getUnseenNotification/{userId}")
    Call<NotificationDomain[]> getUnseenNotification(@Path("userId")Object userId);

    @PUT(NOTIFICATION + "/updateSeenMessage/{messageId}")
    Call<NotificationDomain> updateSeenMessage(@Path("messageId")Object messageId);
}
