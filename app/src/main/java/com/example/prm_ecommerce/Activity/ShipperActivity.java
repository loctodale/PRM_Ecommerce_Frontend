package com.example.prm_ecommerce.Activity;

import android.os.Bundle;

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

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityShipperBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        DeliveryService = DeliveryRepository.getService();
        initRecycleView();

    }

    private void initRecycleView() {
        ArrayList<DeliveryModel> listDeli = new ArrayList<>();
        Call<DeliveryModel[]> call = DeliveryService.getByShipperId("672719d797d0aa83ebf3d0f3");
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