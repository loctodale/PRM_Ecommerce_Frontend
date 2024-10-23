package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.example.prm_ecommerce.API.Interface.IProductService;
import com.example.prm_ecommerce.API.Repository.ProductRepository;
import com.example.prm_ecommerce.Adapter.PopularAdapter;
import com.example.prm_ecommerce.CustomToast;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityMainBinding;
import com.example.prm_ecommerce.domain.ItemCartDomain;
import com.example.prm_ecommerce.domain.ProductDomain;

import java.util.ArrayList;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MainActivity extends AppCompatActivity {
    ActivityMainBinding binding;
    IProductService ProductService;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
//        EdgeToEdge.enable(this);
        ProductService = ProductRepository.getProductService();
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        statusBarColor();
        initRecyclerView();

        bottomNavigation();
    }

    private void bottomNavigation() {
        binding.cartBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                startActivity(new Intent(MainActivity.this, CartActivity.class));
            }
        });
    }

    private void statusBarColor() {
        Window window = MainActivity.this.getWindow();
        window.setStatusBarColor(ContextCompat.getColor(MainActivity.this, R.color.purple_Dark));
    }

    private void initRecyclerView() {
        ArrayList<ProductDomain> items = new ArrayList<>();
        Call<ProductDomain[]> call = ProductService.getAllProducts();
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