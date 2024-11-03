package com.example.prm_ecommerce.Activity;

import android.os.Bundle;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.prm_ecommerce.API.Interface.IDeliveryService;
import com.example.prm_ecommerce.API.Interface.IOrderDetailService;
import com.example.prm_ecommerce.API.Interface.IOrderService;
import com.example.prm_ecommerce.API.Repository.DeliveryRepository;
import com.example.prm_ecommerce.API.Repository.OrderDetailRepository;
import com.example.prm_ecommerce.API.Repository.OrderRepository;
import com.example.prm_ecommerce.Adapter.OrderDetailAdapter;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityOrderDetailBinding;
import com.example.prm_ecommerce.domain.DeliveryDomain;
import com.example.prm_ecommerce.domain.OrderDetailDomain;
import com.example.prm_ecommerce.domain.OrderDomain;

import java.util.ArrayList;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class OrderDetailActivity extends AppCompatActivity {
    private String orderId;
    private IOrderService orderService;
    private IOrderDetailService orderDetailService;
    private IDeliveryService deliveryService;

    private ArrayList<OrderDetailDomain> listOrderDetail;
    private TextView tvAddress, tvDeliveryFee, tvSubtotal, tvTotal, tvStatus;
    private RecyclerView rvProducts;
    private OrderDomain orderDomain;
    private DeliveryDomain deliveryDomain;

    private ActivityOrderDetailBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_order_detail);

        orderService = OrderRepository.getService();
        orderDetailService = OrderDetailRepository.getService();
        deliveryService = DeliveryRepository.getService();

        orderId = (String) getIntent().getSerializableExtra("orderId");

        tvAddress = findViewById(R.id.tvAddress);
        tvDeliveryFee = findViewById(R.id.tvDeliveryFee);
        tvSubtotal = findViewById(R.id.tvSubtotal);
        tvTotal = findViewById(R.id.tvTotal);
        tvStatus = findViewById(R.id.tvStatus);

        getOrderById(orderId);


    }

    private void getOrderById(String orderId){
        Call<OrderDomain> call =  orderService.getById(orderId);
        call.enqueue(new Callback<OrderDomain>() {
            @Override
            public void onResponse(Call<OrderDomain> call, Response<OrderDomain> response) {
                Toast.makeText(OrderDetailActivity.this, "get order by id done", Toast.LENGTH_SHORT).show();
                orderDomain = response.body();
            }

            @Override
            public void onFailure(Call<OrderDomain> call, Throwable throwable) {
                Toast.makeText(OrderDetailActivity.this, "get order by id fail", Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void getDeliveryByOrderId(String orderId){
        Call<DeliveryDomain> call = deliveryService.getByOrderId(orderId);
        call.enqueue(new Callback<DeliveryDomain>() {
            @Override
            public void onResponse(Call<DeliveryDomain> call, Response<DeliveryDomain> response) {
                Toast.makeText(OrderDetailActivity.this, "get delivery by order id done", Toast.LENGTH_SHORT).show();
                deliveryDomain = response.body();
            }

            @Override
            public void onFailure(Call<DeliveryDomain> call, Throwable throwable) {
                Toast.makeText(OrderDetailActivity.this, "get delivery by order id fail", Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void getOrderDetailByOrder(){
        Call<OrderDetailDomain[]> call = orderDetailService.getByOrderId(orderId);
        call.enqueue(new Callback<OrderDetailDomain[]>() {
            @Override
            public void onResponse(Call<OrderDetailDomain[]> call, Response<OrderDetailDomain[]> response) {
                Toast.makeText(OrderDetailActivity.this, "get order detail by order id done", Toast.LENGTH_SHORT).show();
                for (OrderDetailDomain o: response.body()){
                    listOrderDetail.add(o);
                }
            }

            @Override
            public void onFailure(Call<OrderDetailDomain[]> call, Throwable throwable) {
                Toast.makeText(OrderDetailActivity.this, "get order detail by order id fail", Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void ShowOrder(){
        tvAddress.setText(deliveryDomain.getShippingLocation());
        tvDeliveryFee.setText(deliveryDomain.getShippingFee());
        tvSubtotal.setText(orderDomain.getPriceBeforeShip());
        tvTotal.setText(orderDomain.getTotalPrice());
        tvStatus.setText(orderDomain.getStatus());
        binding.rvProducts.setLayoutManager(new LinearLayoutManager(OrderDetailActivity.this, LinearLayoutManager.VERTICAL, false));
        binding.rvProducts.setAdapter(new OrderDetailAdapter(listOrderDetail));
    }


}