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

import com.example.prm_ecommerce.API.Interface.INotificationService;
import com.example.prm_ecommerce.API.Repository.NotificationRepository;
import com.example.prm_ecommerce.Adapter.NotificationAdapter;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityNotificationBinding;
import com.example.prm_ecommerce.domain.NotificationDomain;
import com.example.prm_ecommerce.domain.ProductDomain;

import java.util.ArrayList;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class NotificationActivity extends AppCompatActivity {
    ActivityNotificationBinding binding;
    INotificationService NotificationService;
    private String userId;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityNotificationBinding.inflate(getLayoutInflater());
        NotificationService = NotificationRepository.getNoticationService();
        SharedPreferences sharedPreferences = getSharedPreferences("user_data", MODE_PRIVATE);
        userId = sharedPreferences.getString("user_id", null);
        setContentView(binding.getRoot());
        initRecycleView();
        navigationActivity();
    }

    private void initRecycleView() {
        ArrayList<NotificationDomain> items = new ArrayList<>();
        Call<NotificationDomain[]> call = NotificationService.getNotificationByUserId(userId);
        call.enqueue(new Callback<NotificationDomain[]>() {
            @Override
            public void onResponse(Call<NotificationDomain[]> call, Response<NotificationDomain[]> response) {
                for (NotificationDomain domain : response.body()){
                    items.add(domain);
                }
                binding.notificationListView.setLayoutManager(new LinearLayoutManager(NotificationActivity.this, LinearLayoutManager.VERTICAL, false));
                binding.notificationListView.setAdapter(new NotificationAdapter(items));

            }

            @Override
            public void onFailure(Call<NotificationDomain[]> call, Throwable throwable) {

            }
        });
    }

    private void navigationActivity() {
        binding.backBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(NotificationActivity.this, MainActivity.class);
                NotificationActivity.this.startActivity(intent);
            }
        });

    }
}