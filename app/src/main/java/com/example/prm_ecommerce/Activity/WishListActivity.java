package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.example.prm_ecommerce.API.Interface.IUserService;
import com.example.prm_ecommerce.API.Repository.UserRepository;
import com.example.prm_ecommerce.Adapter.WishlistAdapter;
import com.example.prm_ecommerce.databinding.ActivityWishListBinding;
import com.example.prm_ecommerce.domain.ProductDomain;
import com.example.prm_ecommerce.domain.UserDomain;

import java.util.ArrayList;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class WishListActivity extends AppCompatActivity {
    ActivityWishListBinding binding;
    IUserService UserService;
    String userId;

    @Override
    protected void onRestart() {
        super.onRestart();
        initRecycleView();
        controlNavigation();
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityWishListBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        userId = (String) getIntent().getSerializableExtra("userId");
        UserService = UserRepository.getUserService();
        initRecycleView();
        controlNavigation();
    }

    private void controlNavigation() {
        binding.backBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(WishListActivity.this, MainActivity.class);
                WishListActivity.this.startActivity(intent);
            }
        });
    }

    private void initRecycleView() {
        ArrayList<ProductDomain> items = new ArrayList<>();
        Call<UserDomain> call = UserService.getUserById(userId);

        call.enqueue(new Callback<UserDomain>() {
            @Override
            public void onResponse(Call<UserDomain> call, Response<UserDomain> response) {
                for (ProductDomain product : response.body().getWishList()) {
                    items.add(product);
                }
                binding.wishListView.setLayoutManager(new LinearLayoutManager(WishListActivity.this, LinearLayoutManager.VERTICAL, false));
                binding.wishListView.setAdapter(new WishlistAdapter(items));
            }

            @Override
            public void onFailure(Call<UserDomain> call, Throwable throwable) {

            }
        });
    }

}