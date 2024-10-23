package com.example.prm_ecommerce.Activity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.example.prm_ecommerce.Adapter.CartAdapter;
import com.example.prm_ecommerce.Helper.ChangeNumberItemsListener;
import com.example.prm_ecommerce.Helper.ManagementCart;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityCartBinding;
import com.example.prm_ecommerce.domain.ItemCartDomain;

import java.util.ArrayList;

public class CartActivity extends AppCompatActivity {
    private ManagementCart managementCart;
    ActivityCartBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        managementCart = new ManagementCart(this);
        managementCart.clear();
        addSampleProducts();
        binding = ActivityCartBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        setVariable();
        initList();
    }
    private void addSampleProducts() {
        ItemCartDomain product1 = new ItemCartDomain(
                "1",
                new ArrayList<>(),  // Không có ảnh
                "Product 1",
                200.0  // Giá 200
        );

        ItemCartDomain product2 = new ItemCartDomain(
                "2",
                new ArrayList<>(),  // Không có ảnh
                "Product 2",
                150.0  // Giá 150
        );

        ItemCartDomain product3 = new ItemCartDomain(
                "3",
                new ArrayList<>(),  // Không có ảnh
                "Product 3",
                190.0  // Giá 150
        );

        managementCart.insertItem(product1);  // Thêm sản phẩm 1
        managementCart.insertItem(product2);  // Thêm sản phẩm 2
        managementCart.insertItem(product3);  // Thêm sản phẩm 2
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

        double itemTotal = Math.round(managementCart.getTotalFee() * 100) / 100;
        double total = Math.round(itemTotal+delivery);

        binding.tvDelivery.setText(delivery + "VND");
        binding.tvSubtotal.setText(itemTotal + "VND");
        binding.tvTotal.setText(total + "VND");
    }
    private void setVariable() {
        binding.backBtn.setOnClickListener(v -> finish());
    }
}