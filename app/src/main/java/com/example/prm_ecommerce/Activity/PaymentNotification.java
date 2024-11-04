package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.prm_ecommerce.API.Interface.ICartService;
import com.example.prm_ecommerce.API.Repository.CartRepository;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.domain.CartDomain;
import com.example.prm_ecommerce.domain.ItemCartDomain;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class PaymentNotification extends AppCompatActivity {
    TextView tvThongBao;
    Button btnVeTrangChu;
    String userId;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_payment_notification);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        tvThongBao = findViewById(R.id.tvThongBao);
        btnVeTrangChu =findViewById(R.id.btnVeTrangChu);

        SharedPreferences sharedPreferences = getSharedPreferences("LogInInfo", MODE_PRIVATE);
        userId = sharedPreferences.getString("UserId", null);

        Intent intent = getIntent();
        tvThongBao.setText(intent.getStringExtra("result"));


        btnVeTrangChu.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(PaymentNotification.this, MainActivity.class);
                startActivity(intent);
            }
        });
    }





}