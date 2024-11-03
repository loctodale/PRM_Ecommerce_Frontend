package com.example.prm_ecommerce.API.Interface;

import com.example.prm_ecommerce.domain.OrderDetailDomain;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

public interface IOrderDetailService {
    String ORDERDETAILS = "orderDetails";

    @GET(ORDERDETAILS)
    Call<OrderDetailDomain[]> getAll();

    @GET(ORDERDETAILS + "/{id}")
    Call<OrderDetailDomain> getById(@Path("id")Object id);

    @GET(ORDERDETAILS + "/order/{id}")
    Call<OrderDetailDomain[]> getByOrderId(@Path("id")Object id);

    @POST(ORDERDETAILS)
    Call<OrderDetailDomain> create(@Body OrderDetailDomain orderDetail);

    @PUT(ORDERDETAILS + "/{id}")
    Call<OrderDetailDomain> update(@Path("id")Object id, @Body OrderDetailDomain orderDetail);

    @DELETE(ORDERDETAILS + "/{id}")
    Call<OrderDetailDomain> delete(@Path("id") Object id);
}
