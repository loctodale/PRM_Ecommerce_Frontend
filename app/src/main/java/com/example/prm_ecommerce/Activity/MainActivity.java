package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.StrictMode;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.example.prm_ecommerce.API.Interface.IProductService;
import com.example.prm_ecommerce.API.Repository.ProductRepository;
import com.example.prm_ecommerce.Adapter.PopularAdapter;
import com.example.prm_ecommerce.Helper.RegisterForPushNotificationsAsync;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityMainBinding;
import com.example.prm_ecommerce.domain.ProductDomain;

import java.util.ArrayList;
import java.util.concurrent.Executors;

import me.pushy.sdk.Pushy;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import vn.zalopay.sdk.Environment;
import vn.zalopay.sdk.ZaloPaySDK;

public class MainActivity extends AppCompatActivity {
    ActivityMainBinding binding;
    IProductService ProductService;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        StrictMode.ThreadPolicy policy = new
                StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);

        // ZaloPay SDK Init
        ZaloPaySDK.init(2553, Environment.SANDBOX);
        Pushy.listen(this);

        ProductService = ProductRepository.getProductService();
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        statusBarColor();
        initRecyclerView();
        categoryNavigation();
        bottomNavigation();

        // Register for Pushy notifications
        new RegisterForPushNotificationsAsync(this).execute();

        // Subscribe to topic in the background
        Executors.newSingleThreadExecutor().execute(() -> {
            try {
                Pushy.subscribe("news", getApplicationContext());
                Log.d("Pushy", "Subscribed to 'news' topic");
            } catch (Exception e) {
                Log.e("Pushy", "Failed to subscribe to 'news' topic", e);
            }
        });
    }

    private void categoryNavigation() {
        binding.categoryPhone.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, ListItemInCategoryActivity.class);
                intent.putExtra("categoryId", "671607ee0d68c0aaa6427e6f");
                MainActivity.this.startActivity(intent);
            }
        });
        binding.categoryTools.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, ListItemInCategoryActivity.class);
                intent.putExtra("categoryId", "671cb1de50d7525ca222da9a");
                MainActivity.this.startActivity(intent);
            }
        });
        binding.categoryPC.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, ListItemInCategoryActivity.class);
                intent.putExtra("categoryId", "671cb3ca50d7525ca222daa6");
                MainActivity.this.startActivity(intent);
            }
        });
        binding.categoryLaptop.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, ListItemInCategoryActivity.class);
                intent.putExtra("categoryId", "671cb64b50d7525ca222dab8");
                MainActivity.this.startActivity(intent);
            }
        });
        binding.seeAllTextview.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, ListItemInCategoryActivity.class);
                intent.putExtra("categoryId", "getall");
                MainActivity.this.startActivity(intent);
            }
        });
    }

    private void bottomNavigation() {
        binding.cartBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(MainActivity.this, CartActivity.class);
                // Lưu thông tin đăng nhập
                SharedPreferences sharedPreferences = getSharedPreferences("LogInInfo", MODE_PRIVATE);
                SharedPreferences.Editor editor = sharedPreferences.edit();
                editor.putString("UserId", "6718be16b762285e2490aae2");
                editor.apply();
                MainActivity.this.startActivity(intent);
            }
        });
        binding.wishListBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, WishListActivity.class);
                intent.putExtra("userId", "6718be16b762285e2490aae2");
                MainActivity.this.startActivity(intent);
            }
        });
    }

    private void statusBarColor() {
        Window window = MainActivity.this.getWindow();
        window.setStatusBarColor(ContextCompat.getColor(MainActivity.this, R.color.purple_Dark));
    }

    private void initRecyclerView() {
        ArrayList<ProductDomain> items = new ArrayList<>();
        Call<ProductDomain[]> call = ProductService.getPopularProducts();
        call.enqueue(new Callback<ProductDomain[]>() {
            @Override
            public void onResponse(Call<ProductDomain[]> call, Response<ProductDomain[]> response) {
                if (response.isSuccessful()) {
                    ProductDomain[] productDomains = response.body();
                    if (productDomains != null) {
//                        CustomToast.makeText(MainActivity.this, "Response body is null", CustomToast.LENGTH_LONG, CustomToast.SUCCESS,true).show();
                        Log.d("Loi", "Received " + productDomains.length + " products");
                        // TODO: Process the received products
                        for(ProductDomain product : productDomains){
                            items.add(product);
                        }

                        binding.PopularView.setLayoutManager(new LinearLayoutManager(MainActivity.this, LinearLayoutManager.HORIZONTAL, false));
                        binding.PopularView.setAdapter(new PopularAdapter(items));
                    } else {
                        Toast.makeText(MainActivity.this, "Response body is null", Toast.LENGTH_SHORT).show();
                        Log.e("Success", "Response body is null");
                    }
                } else {
                    Toast.makeText(MainActivity.this, "Error: " + response.code(), Toast.LENGTH_SHORT).show();
                    Log.e("Loi", "Error: " + response.code() + " " + response.message());
                }
            }

            @Override
            public void onFailure(Call<ProductDomain[]> call, Throwable throwable) {
                Toast.makeText(MainActivity.this, "Network error: " + throwable.getMessage(), Toast.LENGTH_SHORT).show();
                Log.e("Error", "Network error", throwable);
            }
        });
    }
}