package com.example.prm_ecommerce.API.Interface;

import com.example.prm_ecommerce.Model.RequestAddProductToCartModel;
import com.example.prm_ecommerce.domain.CartDomain;
import com.example.prm_ecommerce.domain.ProductDomain;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

public interface ICartService {
    String CART = "carts";

    @GET(CART)
    Call<CartDomain[]> getAllCarts();

    @GET(CART + "/${id}")
    Call<CartDomain> getCartById(@Path("id")Object id);

    @POST(CART)
    Call<CartDomain> addProductToCart(@Body RequestAddProductToCartModel request);

    @PUT(CART + "/{id}")
    Call<CartDomain> updateCart(@Path("id")Object id, @Body ProductDomain product);

    @DELETE(CART + "/{id}")
    Call<CartDomain> deleteCart(@Path("id") Object id);
}
