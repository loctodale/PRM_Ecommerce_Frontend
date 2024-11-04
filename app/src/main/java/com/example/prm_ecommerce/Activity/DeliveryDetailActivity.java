package com.example.prm_ecommerce.Activity;

import android.os.Bundle;
import android.view.View;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.prm_ecommerce.API.Interface.IDeliveryService;
import com.example.prm_ecommerce.API.Repository.DeliveryRepository;
import com.example.prm_ecommerce.Model.DeliveryModel;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityDeliveryDetailBinding;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class DeliveryDetailActivity extends AppCompatActivity {
    ActivityDeliveryDetailBinding binding;
    IDeliveryService DeliveryService;
    private String deliveryId;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityDeliveryDetailBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        DeliveryService = DeliveryRepository.getService();
        deliveryId = (String) getIntent().getSerializableExtra("deliveryId");

        initView();
        btnAction();
    }

    private void btnAction() {
        binding.backBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });
        binding.updateBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Call<String> call = DeliveryService.updateShipment(deliveryId);
                call.enqueue(new Callback<String>() {
                    @Override
                    public void onResponse(Call<String> call, Response<String> response) {
                        initView();
                    }

                    @Override
                    public void onFailure(Call<String> call, Throwable throwable) {

                    }
                });
            }
        });
    }

    private void initView() {
        Call<DeliveryModel> call = DeliveryService.getById(deliveryId);
        call.enqueue(new Callback<DeliveryModel>() {

            @Override
            public void onResponse(Call<DeliveryModel> call, Response<DeliveryModel> response) {
                binding.tvAddress.setText(response.body().getShippingLocation());
                binding.tvDeliveryFee.setText(response.body().getShippingFee()+"");
                binding.tvSubtotal.setText(response.body().getOrder().getPriceBeforeShip()+"");
                binding.tvTotal.setText(response.body().getOrder().getTotalPrice()+"");
                binding.tvStatus.setText(response.body().getStatus());
            }

            @Override
            public void onFailure(Call<DeliveryModel> call, Throwable throwable) {

            }
        });
    }
}