package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.example.prm_ecommerce.API.Interface.IProductService;
import com.example.prm_ecommerce.API.Repository.ProductRepository;
import com.example.prm_ecommerce.Adapter.ListItemInCategoryAdapter;
import com.example.prm_ecommerce.Adapter.PopularAdapter;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityListItemInCategoryBinding;
import com.example.prm_ecommerce.domain.ProductDomain;

import java.util.ArrayList;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ListItemInCategoryActivity extends AppCompatActivity {
    ActivityListItemInCategoryBinding binding;
    IProductService ProductService;
    String categoryId;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityListItemInCategoryBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        ProductService = ProductRepository.getProductService();
        categoryId = (String) getIntent().getSerializableExtra("categoryId");
        actionBarBinding();
        initRecycleView();
    }

    private void actionBarBinding() {
        binding.backBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(ListItemInCategoryActivity.this, MainActivity.class);
                ListItemInCategoryActivity.this.startActivity(intent);
            }
        });
        switch (categoryId) {
            case "671607ee0d68c0aaa6427e6f":
                binding.categoryTxt.setText("Phone");
                break;
            case "671cb1de50d7525ca222da9a":
                binding.categoryTxt.setText("Tools");
                break;
            case "671cb3ca50d7525ca222daa6":
                binding.categoryTxt.setText("PC");
                break;
            case "671cb64b50d7525ca222dab8":
                binding.categoryTxt.setText("Laptop");
                break;
            default: binding.categoryTxt.setText("All Product");
        }
    }

    private void initRecycleView() {
        ArrayList<ProductDomain> items = new ArrayList<>();
        if(!categoryId.equals("getall")) {
            Call<ProductDomain[]> call = ProductService.getProductByCategoryId(categoryId);
            call.enqueue(new Callback<ProductDomain[]>() {
                @Override
                public void onResponse(Call<ProductDomain[]> call, Response<ProductDomain[]> response) {
                    if (response.isSuccessful()) {
                        ProductDomain[] productDomains = response.body();
                        if (productDomains != null) {
//                        CustomToast.makeText(MainActivity.this, "Response body is null", CustomToast.LENGTH_LONG, CustomToast.SUCCESS,true).show();
                            Log.d("Loi", "Received " + productDomains.length + " products");
                            // TODO: Process the received products
                            for (ProductDomain product : productDomains) {
                                items.add(product);
                            }

                            binding.listItemInCategoryList.setLayoutManager(new GridLayoutManager(ListItemInCategoryActivity.this, 2));
                            binding.listItemInCategoryList.setAdapter(new ListItemInCategoryAdapter(items));
                        } else {
                            Toast.makeText(ListItemInCategoryActivity.this, "Response body is null", Toast.LENGTH_SHORT).show();
                            Log.e("Success", "Response body is null");
                        }
                    } else {
                        Toast.makeText(ListItemInCategoryActivity.this, "Error: " + response.code(), Toast.LENGTH_SHORT).show();
                        Log.e("Loi", "Error: " + response.code() + " " + response.message());
                    }
                }

                @Override
                public void onFailure(Call<ProductDomain[]> call, Throwable throwable) {
                    Toast.makeText(ListItemInCategoryActivity.this, "Network error: " + throwable.getMessage(), Toast.LENGTH_SHORT).show();
                    Log.e("Error", "Network error", throwable);
                }
            });
        } else {
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
                            for (ProductDomain product : productDomains) {
                                items.add(product);
                            }

                            binding.listItemInCategoryList.setLayoutManager(new GridLayoutManager(ListItemInCategoryActivity.this, 2));
                            binding.listItemInCategoryList.setAdapter(new ListItemInCategoryAdapter(items));
                        } else {
                            Toast.makeText(ListItemInCategoryActivity.this, "Response body is null", Toast.LENGTH_SHORT).show();
                            Log.e("Success", "Response body is null");
                        }
                    } else {
                        Toast.makeText(ListItemInCategoryActivity.this, "Error: " + response.code(), Toast.LENGTH_SHORT).show();
                        Log.e("Loi", "Error: " + response.code() + " " + response.message());
                    }
                }

                @Override
                public void onFailure(Call<ProductDomain[]> call, Throwable throwable) {
                    Toast.makeText(ListItemInCategoryActivity.this, "Network error: " + throwable.getMessage(), Toast.LENGTH_SHORT).show();
                    Log.e("Error", "Network error", throwable);
                }
            });
        }
    }
}