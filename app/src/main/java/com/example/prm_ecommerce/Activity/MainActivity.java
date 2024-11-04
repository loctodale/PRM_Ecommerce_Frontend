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

import com.example.prm_ecommerce.API.Interface.INotificationService;
import com.example.prm_ecommerce.API.Interface.IProductService;
import com.example.prm_ecommerce.API.Repository.NotificationRepository;
import com.example.prm_ecommerce.API.Repository.ProductRepository;
import com.example.prm_ecommerce.Adapter.PopularAdapter;
import com.example.prm_ecommerce.Helper.RegisterForPushNotificationsAsync;
import com.example.prm_ecommerce.Model.LoginSession;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityMainBinding;
import com.example.prm_ecommerce.domain.NotificationDomain;
import com.example.prm_ecommerce.domain.ProductDomain;
//import com.google.firebase.FirebaseApp;

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
    INotificationService NotificationService;
    String userId;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        StrictMode.ThreadPolicy policy = new
                StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);

        // ZaloPay SDK Init
        ZaloPaySDK.init(2553, Environment.SANDBOX);
        Pushy.listen(this);
//        FirebaseApp.initializeApp(this);

        ProductService = ProductRepository.getProductService();
        NotificationService = NotificationRepository.getNoticationService();
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        SharedPreferences sharedPreferences = getSharedPreferences("user_data", MODE_PRIVATE);
        userId = sharedPreferences.getString("user_id", null);
        setContentView(binding.getRoot());
        statusBarColor();
        initRecyclerView();
        categoryNavigation();
        bottomNavigation();
        controlNavigation();
//        loginOrProfileSwitch();
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
    // chuyen doi login or profile
    private void loginOrProfileSwitch(){
        binding.imageView85.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                // Lấy giá trị của session token và user name
                String userId = LoginSession.userId;
                if (userId != null) {
                    // Người dùng đã đăng nhập
                    Intent intent = new Intent(MainActivity.this, UserProfileActitvity.class);
                    startActivity(intent);
                } else {
                    // Người dùng chưa đăng nhập
                    Intent intent = new Intent(MainActivity.this, LoginActivity.class);
                    startActivity(intent);
                }
             /*   SharedPreferences sharedPreferences = getSharedPreferences("user_data", MODE_PRIVATE);
                String userId = sharedPreferences.getString("user_id", null);
                if(userId!= null){
                    Intent intent = new Intent(MainActivity.this , UserProfileActitvity.class);
                    startActivity(intent);
                }else{
                    Intent intent = new Intent(MainActivity.this , LoginActivity.class);
                    startActivity(intent);
                }*/
            }
        });
    }
    private void controlNavigation() {
        binding.btnNotification.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, NotificationActivity.class);
                MainActivity.this.startActivity(intent);
            }
        });
        binding.fabChat.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(MainActivity.this, ChatActivity.class);
                MainActivity.this.startActivity(intent);
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


                editor.putString("UserId", userId);
                editor.apply();
                MainActivity.this.startActivity(intent);
            }
        });
        binding.wishListBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, WishListActivity.class);
                intent.putExtra("userId", userId);
                MainActivity.this.startActivity(intent);
            }
        });
        binding.myOrder.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(MainActivity.this, ListOrderOfUserActivity.class);
                MainActivity.this.startActivity(intent);
            }
        });
    }

    private void statusBarColor() {
        Window window = MainActivity.this.getWindow();
        window.setStatusBarColor(ContextCompat.getColor(MainActivity.this, R.color.purple_Dark));
    }

    private void initRecyclerView() {
        callPopularProduct();
        callUnseenNotification();
    }

    private void callUnseenNotification() {
        Call<NotificationDomain[]> call = NotificationService.getUnseenNotification(userId);
        call.enqueue(new Callback<NotificationDomain[]>() {
            @Override
            public void onResponse(Call<NotificationDomain[]> call, Response<NotificationDomain[]> response) {
                binding.notificationTextview.setText(response.body().length+ "");
            }

            @Override
            public void onFailure(Call<NotificationDomain[]> call, Throwable throwable) {

            }
        });
    }

    private void callPopularProduct() {
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