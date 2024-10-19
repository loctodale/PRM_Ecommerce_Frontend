package com.example.prm_ecommerce.API.Interface;

import com.example.prm_ecommerce.Model.ProductModel;

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
    Call<ProductModel[]> getAllProducts();

    @GET(PRODUCT + "/${id}/")
    Call<ProductModel> getProductById(@Path("id")Object id);

    @POST(PRODUCT)
    Call<ProductModel> createProduct(@Body ProductModel product);

    @PUT(PRODUCT + "/{id}")
    Call<ProductModel> updateProduct(@Path("id")Object id, @Body ProductModel product);

    @DELETE(PRODUCT + "/{id}")
    Call<ProductModel> deleteProduct(@Path("id") Object id);
}
