package com.example.prm_ecommerce.API.Interface;

import com.example.prm_ecommerce.domain.ProductDomain;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

public interface IProductService {
    String PRODUCT = "products";

    @GET(PRODUCT)
    Call<ProductDomain[]> getAllProducts();

    @GET(PRODUCT + "/${id}")
    Call<ProductDomain> getProductById(@Path("id")Object id);

    @POST(PRODUCT)
    Call<ProductDomain> createProduct(@Body ProductDomain product);

    @PUT(PRODUCT + "/{id}")
    Call<ProductDomain> updateProduct(@Path("id")Object id, @Body ProductDomain product);

    @DELETE(PRODUCT + "/{id}")
    Call<ProductDomain> deleteProduct(@Path("id") Object id);
}
