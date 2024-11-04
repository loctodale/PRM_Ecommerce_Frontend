package com.example.prm_ecommerce.API.Interface;

import com.example.prm_ecommerce.Model.RequestUpdateWishList;
import com.example.prm_ecommerce.domain.CartDomain;
import com.example.prm_ecommerce.domain.LoginDomain;
import com.example.prm_ecommerce.domain.LoginRequest;
import com.example.prm_ecommerce.domain.ProductDomain;
import com.example.prm_ecommerce.domain.ResponseFirebaseDomain;
import com.example.prm_ecommerce.domain.UserDomain;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

public interface IUserService {
    String USER = "users";

    @GET(USER + "/{id}")
    Call<UserDomain> getUserById(@Path("id")Object userId);

    @POST(USER)
    Call<UserDomain> createUser(@Body UserDomain user);

    @POST("auth/login")
    Call<UserDomain> loginUser(@Body LoginDomain login);

    @PUT(USER + "/{id}")
    Call<UserDomain> updateUser(@Path("id")Object id, @Body UserDomain user);

    @POST(USER + "/updateWishList")
    Call<UserDomain> toggleWishList(@Body RequestUpdateWishList request);

    @POST("auth/registerFirebase")
    Call<UserDomain> registerFirebase(@Body UserDomain user);

    @POST("auth/loginFirebase")
    Call<ResponseFirebaseDomain> loginFirebase(@Body LoginRequest loginRequest);
}
