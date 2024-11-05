package com.example.prm_ecommerce.API.Interface;

import com.example.prm_ecommerce.Model.RequestCreateOrderStackModel;
import com.example.prm_ecommerce.domain.OrderDetailDomain;
import com.example.prm_ecommerce.domain.OrderDomain;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

public interface IOrderService {
    String ORDERS = "orders";

    @GET(ORDERS)
    Call<OrderDomain[]> getAll();

    @GET(ORDERS + "/{id}")
    Call<OrderDomain> getById(@Path("id")Object id);

    @GET(ORDERS + "/user/{userId}")
    Call<OrderDomain[]> getOrderByUserId(@Path("id")Object userId);

    @POST(ORDERS)
    Call<OrderDomain> create(@Body OrderDomain order);
    @POST(ORDERS + "/createOrderStack")
    Call<String> createOrderStack(@Body RequestCreateOrderStackModel order);

    @PUT(ORDERS + "/{id}")
    Call<OrderDomain> update(@Path("id")Object id, @Body OrderDomain order);

    @DELETE(ORDERS + "/{id}")
    Call<OrderDomain> delete(@Path("id") Object id);
}
