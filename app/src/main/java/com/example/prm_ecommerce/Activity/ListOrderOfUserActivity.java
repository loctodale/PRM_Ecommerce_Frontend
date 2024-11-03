package com.example.prm_ecommerce.Activity;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.example.prm_ecommerce.API.Interface.ICartService;
import com.example.prm_ecommerce.API.Interface.IOrderService;
import com.example.prm_ecommerce.API.Interface.IProductService;
import com.example.prm_ecommerce.API.Repository.CartRepository;
import com.example.prm_ecommerce.API.Repository.OrderRepository;
import com.example.prm_ecommerce.API.Repository.ProductRepository;
import com.example.prm_ecommerce.Adapter.ListItemInCategoryAdapter;
import com.example.prm_ecommerce.Adapter.ListOrderAdapter;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityListItemInCategoryBinding;
import com.example.prm_ecommerce.databinding.ActivityListOrderOfUserBinding;
import com.example.prm_ecommerce.domain.CartDomain;
import com.example.prm_ecommerce.domain.OrderDomain;
import com.example.prm_ecommerce.domain.OrderDomain;

import java.util.ArrayList;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ListOrderOfUserActivity extends AppCompatActivity {
    ActivityListOrderOfUserBinding binding;
    IOrderService orderService;
    String userId;
    ImageView btnBack;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_list_order_of_user);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        SharedPreferences sharedPreferences = getSharedPreferences("LogInInfo", MODE_PRIVATE);
        userId = sharedPreferences.getString("UserId", null);

        binding = ActivityListOrderOfUserBinding.inflate(getLayoutInflater());
        btnBack = findViewById(R.id.btnBack);
        setContentView(binding.getRoot());
        orderService = OrderRepository.getService();

        initRecycleView();
    }
    
    private void initRecycleView(){
        ArrayList<OrderDomain> items = new ArrayList<>();
        Call<OrderDomain[]> call = orderService.getAll();
        call.enqueue(new Callback<OrderDomain[]>() {
            @Override
            public void onResponse(Call<OrderDomain[]> call, Response<OrderDomain[]> response) {
                if (response.isSuccessful()) {
                    OrderDomain[] OrderDomains = response.body();
                    if (OrderDomains != null) {
                        Log.d("Loi", "Received " + OrderDomains.length + " products");
                        for (OrderDomain order : OrderDomains) {
                            items.add(order);
                        }

                        binding.rvListOrder.setLayoutManager(new LinearLayoutManager(ListOrderOfUserActivity.this));
                        binding.rvListOrder.setAdapter(new ListOrderAdapter(items));
                    } else {
                        Toast.makeText(ListOrderOfUserActivity.this, "Response body is null", Toast.LENGTH_SHORT).show();
                        Log.e("Success", "Response body is null");
                    }
                } else {
                    Toast.makeText(ListOrderOfUserActivity.this, "Error: " + response.code(), Toast.LENGTH_SHORT).show();
                    Log.e("Loi", "Error: " + response.code() + " " + response.message());
                }
            }

            @Override
            public void onFailure(Call<OrderDomain[]> call, Throwable throwable) {
                Toast.makeText(ListOrderOfUserActivity.this, "Network error: " + throwable.getMessage(), Toast.LENGTH_SHORT).show();
                Log.e("Error", "Network error", throwable);
            }
        });
    }
}