package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.StrictMode;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.ImageView;

import androidx.activity.EdgeToEdge;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.example.prm_ecommerce.API.CreateOrder;
import com.example.prm_ecommerce.API.Interface.ICartService;
import com.example.prm_ecommerce.API.Repository.CartRepository;
import com.example.prm_ecommerce.Adapter.CartAdapter;
import com.example.prm_ecommerce.Helper.ChangeNumberItemsListener;
import com.example.prm_ecommerce.Helper.ManagementCart;
import com.example.prm_ecommerce.Model.ItemInCartModel;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityCartBinding;
import com.example.prm_ecommerce.domain.CartDomain;
import com.example.prm_ecommerce.domain.ItemCartDomain;
import com.example.prm_ecommerce.domain.ProductDomain;

import org.json.JSONObject;

import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Currency;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import vn.zalopay.sdk.Environment;
import vn.zalopay.sdk.ZaloPayError;
import vn.zalopay.sdk.ZaloPaySDK;
import vn.zalopay.sdk.listeners.PayOrderListener;

public class CartActivity extends AppCompatActivity {
    private ManagementCart managementCart;
    ActivityCartBinding binding;
    private String userId;
    private ICartService CartService;
    ImageView ivAddAddress;
    Button btnOrder;

    TextView tvAddress;
    TextView tvDeliveryFee;

    String address;
    String deliveryFee;
    String totalString;
    double delivery = 0;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        CartService = CartRepository.getCartService();
        managementCart = new ManagementCart(this);
//        userId = (String) getIntent().getSerializableExtra("userId");
        SharedPreferences sharedPreferences = getSharedPreferences("LogInInfo", MODE_PRIVATE);
        userId = sharedPreferences.getString("UserId", null);
        addSampleProducts();
        binding = ActivityCartBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        setVariable();
        initList();
        btnOrder = findViewById(R.id.btnOrderNow);
        tvAddress = findViewById(R.id.tvAddress);
        tvDeliveryFee = findViewById(R.id.tvDeliveryFee);

        setAddressAndDeliveryFee();

        ivAddAddress = (ImageView) findViewById(R.id.ivAddAddress);
        ivAddAddress.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                addAddress();
            }
        });

        btnOrder.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Toast.makeText(CartActivity.this, "click", Toast.LENGTH_SHORT).show();
                CreateOrder orderApi = new CreateOrder();

                try {
                    JSONObject data = orderApi.createOrder(totalString);
                    Log.d("CreateOrder", "Response Data: " + data.toString());
                    String code = data.getString("return_code");

                    if (code.equals("1")) {
                        String token = data.getString("zp_trans_token");
                        ZaloPaySDK.getInstance().payOrder( CartActivity.this, token, "demozpdk://app", new PayOrderListener() {
                            @Override
                            public void onPaymentSucceeded(String s, String s1, String s2) {
                                Intent intent1 = new Intent(CartActivity.this, PaymentNotification.class);
                                intent1.putExtra("result", "Thanh toan thanh cong");
                                startActivity(intent1);
                            }

                            @Override
                            public void onPaymentCanceled(String s, String s1) {
                                Intent intent1 = new Intent(CartActivity.this, PaymentNotification.class);
                                intent1.putExtra("result", "Huy thanh toan");
                                startActivity(intent1);

                            }

                            @Override
                            public void onPaymentError(ZaloPayError zaloPayError, String s, String s1) {
                                Intent intent1 = new Intent(CartActivity.this, PaymentNotification.class);
                                intent1.putExtra("result", "Loi thanh toan");
                                startActivity(intent1);

                            }
                        });
                    }

                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    private void setAddressAndDeliveryFee(){
        String getAddress = getIntent().getStringExtra("ADDRESS");
        String getFee = getIntent().getStringExtra("DELIVERY_FEE");

        NumberFormat format = NumberFormat.getCurrencyInstance();
        format.setMaximumFractionDigits(0);
        format.setCurrency(Currency.getInstance("VND"));
        if (getAddress!=null && getFee!=null){
            address = getAddress;
            delivery = Double.valueOf(getFee);
            tvAddress.setText(address);
            calculatorCart();
        } else {
            // Xử lý trường hợp không có địa chỉ hoặc phí
            tvAddress.setText("Address");
            tvDeliveryFee.setText("0 VND");
        }

    }

    private void addAddress(){
        startActivity(new Intent(CartActivity.this, AddAddressActivity.class));
    }

    private void addSampleProducts() {
        Call<CartDomain> call = CartService.getCartByUserId(userId);
        call.enqueue(new Callback<CartDomain>() {
            @Override
            public void onResponse(Call<CartDomain> call, Response<CartDomain> response) {
                managementCart.clear();

                for (ItemInCartModel product : response.body().getProducts()) {
                    managementCart.insertItem(new ItemCartDomain(
                            product.getProduct().get_id(),
                            product.getProduct().getName(),
                            product.getProduct().getImage(),
                            product.getQuantity(),
                            product.getProduct().getPrice()));
                }
                calculatorCart();
            }

            @Override
            public void onFailure(Call<CartDomain> call, Throwable throwable) {
                Toast.makeText(CartActivity.this, throwable.getMessage(), Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void initList() {
        Call<CartDomain> call = CartService.getCartByUserId(userId);
        call.enqueue(new Callback<CartDomain>() {
            @Override
            public void onResponse(Call<CartDomain> call, Response<CartDomain> response) {
                ArrayList<ItemCartDomain> items = new ArrayList<>();

                for (ItemInCartModel product : response.body().getProducts()) {
                    items.add(new ItemCartDomain(
                            product.getProduct().get_id(),
                            product.getProduct().getName(),
                            product.getProduct().getImage(),
                            product.getQuantity(),
                            product.getProduct().getPrice()));
                }
                if (items.isEmpty()) {
                    binding.tvEmpty.setVisibility(View.VISIBLE);
                    binding.scroll.setVisibility(View.GONE);
                } else {
                    binding.tvEmpty.setVisibility(View.GONE);
                    binding.scroll.setVisibility(View.VISIBLE);
                }

                binding.cartView.setLayoutManager(new LinearLayoutManager(CartActivity.this, LinearLayoutManager.VERTICAL, false));
                binding.cartView.setAdapter(new CartAdapter(items, new ChangeNumberItemsListener() {
                    @Override
                    public void change() {
                        calculatorCart();
                    }
                }));

            }

            @Override
            public void onFailure(Call<CartDomain> call, Throwable throwable) {
                Toast.makeText(CartActivity.this, throwable.getMessage(), Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void calculatorCart() {
        NumberFormat format = NumberFormat.getCurrencyInstance();
        format.setMaximumFractionDigits(0);
        format.setCurrency(Currency.getInstance("VND"));
        double itemTotal = Math.round(managementCart.getTotalFee() * 100) / 100;
        double total = Math.round(itemTotal + delivery);

        binding.tvDeliveryFee.setText(format.format(delivery));
        binding.tvSubtotal.setText(format.format(itemTotal));
        binding.tvTotal.setText(format.format(total));

        totalString = String.valueOf((int)total);
    }

    private void setVariable() {
        binding.backBtn.setOnClickListener(v -> finish());
    }

    @Override
    protected void onNewIntent(@NonNull Intent intent) {
        super.onNewIntent(intent);
        ZaloPaySDK.getInstance().onResult(intent);

        setIntent(intent); // Cập nhật Intent mới
        setAddressAndDeliveryFee();
    }
}