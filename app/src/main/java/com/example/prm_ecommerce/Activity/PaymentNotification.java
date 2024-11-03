package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
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
    private ICartService cartService;
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
        cartService = CartRepository.getCartService();

        SharedPreferences sharedPreferences = getSharedPreferences("LogInInfo", MODE_PRIVATE);
        userId = sharedPreferences.getString("UserId", null);

        Intent intent = getIntent();
        tvThongBao.setText(intent.getStringExtra("result"));

//        deleteCartByUserId();

        btnVeTrangChu.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(PaymentNotification.this, MainActivity.class);
                startActivity(intent);
            }
        });
    }


    private void deleteCartByUserId(){
        Call<CartDomain> call = cartService.deleteCartByUserId(userId);
        call.enqueue(new Callback<CartDomain>() {
            @Override
            public void onResponse(Call<CartDomain> call, Response<CartDomain> response) {
                if (response.isSuccessful()) {
                    Toast.makeText(PaymentNotification.this, "Cart deleted successfully", Toast.LENGTH_SHORT).show();
                } else {
                    // Hiển thị mã lỗi nếu không thành công
                    Toast.makeText(PaymentNotification.this, "Failed Error code: " + response.code(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<CartDomain> call, Throwable throwable) {
                Toast.makeText(PaymentNotification.this, "Fail delete cart", Toast.LENGTH_SHORT).show();

            }
        });
    }

    private void createOrder(){

    }

    private void createOrderDetail(){

    }

    private void createDelivery(){

    }
}