package com.example.prm_ecommerce.Activity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;

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

import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Currency;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class CartActivity extends AppCompatActivity {
    private ManagementCart managementCart;
    ActivityCartBinding binding;
    private String userId;
    private ICartService CartService;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        CartService = CartRepository.getCartService();
        managementCart = new ManagementCart(this);
        addSampleProducts();

//
        binding = ActivityCartBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        setVariable();
        calculatorCart();
        initList();

    }
    private void addSampleProducts() {
//        ItemCartDomain product1 = new ItemCartDomain(
//                "1",
//                new ArrayList<>(),  // Không có ảnh
//                "Product 1",
//                200.0  // Giá 200
//        );
//
//        ItemCartDomain product2 = new ItemCartDomain(
//                "2",
//                new ArrayList<>(),  // Không có ảnh
//                "Product 2",
//                150.0  // Giá 150
//        );
//
//        ItemCartDomain product3 = new ItemCartDomain(
//                "3",
//                new ArrayList<>(),  // Không có ảnh
//                "Product 3",
//                190.0  // Giá 150
//        );
//
//        managementCart.insertItem(product1);  // Thêm sản phẩm 1
//        managementCart.insertItem(product2);  // Thêm sản phẩm 2
//        managementCart.insertItem(product3);  // Thêm sản phẩm 2
        userId = (String) getIntent().getSerializableExtra("userId");
        Call<CartDomain> call = CartService.getCartByUserId(userId);
        call.enqueue(new Callback<CartDomain>() {
            @Override
            public void onResponse(Call<CartDomain> call, Response<CartDomain> response) {
                 managementCart.clear();

                for(ItemInCartModel product : response.body().getProducts()) {
                    managementCart.insertItem(new ItemCartDomain(
                            product.getProduct().get_id(),
                            product.getProduct().getName(),
                            product.getProduct().getImage(),
                            product.getQuantity(),
                            product.getProduct().getPrice()));
                }
            }

            @Override
            public void onFailure(Call<CartDomain> call, Throwable throwable) {
                Toast.makeText(CartActivity.this, throwable.getMessage(), Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void initList() {
         if (managementCart.getListCart().isEmpty()){
            binding.tvEmpty.setVisibility(View.VISIBLE);
            binding.scroll.setVisibility(View.GONE);
         }else {
             binding.tvEmpty.setVisibility(View.GONE);
             binding.scroll.setVisibility(View.VISIBLE);
         }
         binding.cartView.setLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false));
         binding.cartView.setAdapter(new CartAdapter(managementCart.getListCart(), new ChangeNumberItemsListener(){
            @Override
            public void change() {
                calculatorCart();
            }
        }));
    }

    private void calculatorCart(){
        double delivery = 10;
        NumberFormat format = NumberFormat.getCurrencyInstance();
        format.setMaximumFractionDigits(0);
        format.setCurrency(Currency.getInstance("VND"));
        double itemTotal = Math.round(managementCart.getTotalFee() * 100) / 100;
        double total = Math.round(itemTotal+delivery);

        binding.tvDelivery.setText(delivery + "VND");
        binding.tvSubtotal.setText(format.format(itemTotal));
        binding.tvTotal.setText(format.format(total));
    }
    private void setVariable() {
        binding.backBtn.setOnClickListener(v -> finish());
    }
}