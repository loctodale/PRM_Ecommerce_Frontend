package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.example.prm_ecommerce.API.Interface.IDeliveryService;
import com.example.prm_ecommerce.API.Repository.DeliveryRepository;
import com.example.prm_ecommerce.Adapter.DeliveryAdapter;
import com.example.prm_ecommerce.Model.DeliveryModel;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityShipperBinding;

import java.util.ArrayList;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ShipperActivity extends AppCompatActivity {
    ActivityShipperBinding binding;
    IDeliveryService DeliveryService;
    private String userId;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityShipperBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        SharedPreferences sharedPreferences = getSharedPreferences("user_data", MODE_PRIVATE);
        userId = sharedPreferences.getString("user_id", null);
        DeliveryService = DeliveryRepository.getService();
        initRecycleView();

        binding.backBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        binding.refreshBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                initRecycleView();
            }
        });

    }

    private void initRecycleView() {
        ArrayList<DeliveryModel> listDeli = new ArrayList<>();
        Call<DeliveryModel[]> call = DeliveryService.getByShipperId(userId);
        call.enqueue(new Callback<DeliveryModel[]>() {
            @Override
            public void onResponse(Call<DeliveryModel[]> call, Response<DeliveryModel[]> response) {
                for(DeliveryModel model : response.body()) {
                    listDeli.add(model);
                }
                binding.deviveryListView.setLayoutManager(new LinearLayoutManager(ShipperActivity.this, LinearLayoutManager.VERTICAL, false));
                binding.deviveryListView.setAdapter(new DeliveryAdapter(listDeli));
            }

            @Override
            public void onFailure(Call<DeliveryModel[]> call, Throwable throwable) {

            }
        });
    }
}