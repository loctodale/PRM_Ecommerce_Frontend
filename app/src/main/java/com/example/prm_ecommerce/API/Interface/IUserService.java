package com.example.prm_ecommerce.API.Interface;

import com.example.prm_ecommerce.Model.RequestUpdateWishList;
import com.example.prm_ecommerce.domain.CartDomain;
import com.example.prm_ecommerce.domain.UserDomain;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface IUserService {
    String USER = "users";

    @GET(USER + "/{id}")
    Call<UserDomain> getUserById(@Path("id")Object userId);

    @POST(USER + "/updateWishList")
    Call<UserDomain> toggleWishList(@Body RequestUpdateWishList request);
}
