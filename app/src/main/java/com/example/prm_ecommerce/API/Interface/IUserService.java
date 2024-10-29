package com.example.prm_ecommerce.API.Interface;

import com.example.prm_ecommerce.domain.UserDomain;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

public interface IUserService {
    String USER = "user";

    @GET(USER)
    Call<UserDomain[]> getAllProducts();

    @GET(USER + "/${id}/")
    Call<UserDomain> getUserById(@Path("id")Object id);

    @POST(USER)
    Call<UserDomain> createUser(@Body UserDomain product);

    @PUT(USER + "/{id}")
    Call<UserDomain> updateUser(@Path("id")Object id, @Body UserDomain product);

    @DELETE(USER + "/{id}")
    Call<UserDomain> deleteUser(@Path("id") Object id);
}
