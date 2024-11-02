package com.example.prm_ecommerce.API.Interface;

import com.example.prm_ecommerce.domain.DeliveryDomain;
import com.example.prm_ecommerce.domain.OrderDetailDomain;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

public interface IDeliveryService {
    String DELIVERY = "deliverys";

    @GET(DELIVERY)
    Call<DeliveryDomain[]> getAll();

    @GET(DELIVERY + "/{id}")
    Call<DeliveryDomain> getById(@Path("id")Object id);

    @POST(DELIVERY)
    Call<DeliveryDomain> create(@Body DeliveryDomain delivery);

    @PUT(DELIVERY + "/{id}")
    Call<DeliveryDomain> update(@Path("id")Object id, @Body DeliveryDomain delivery);

    @DELETE(DELIVERY + "/{id}")
    Call<DeliveryDomain> delete(@Path("id") Object id);
}
