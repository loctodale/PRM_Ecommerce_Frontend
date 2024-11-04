package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.StrictMode;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.DatePicker;
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
import com.example.prm_ecommerce.API.Interface.IDeliveryService;
import com.example.prm_ecommerce.API.Interface.IOrderDetailService;
import com.example.prm_ecommerce.API.Interface.IOrderService;
import com.example.prm_ecommerce.API.Interface.IProductService;
import com.example.prm_ecommerce.API.Repository.CartRepository;
import com.example.prm_ecommerce.API.Repository.OrderDetailRepository;
import com.example.prm_ecommerce.API.Repository.OrderRepository;
import com.example.prm_ecommerce.API.Repository.ProductRepository;
import com.example.prm_ecommerce.Adapter.CartAdapter;
import com.example.prm_ecommerce.Helper.ChangeNumberItemsListener;
import com.example.prm_ecommerce.Helper.ManagementCart;
import com.example.prm_ecommerce.Model.ItemInCartModel;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityCartBinding;
import com.example.prm_ecommerce.domain.CartDomain;
import com.example.prm_ecommerce.domain.DeliveryDomain;
import com.example.prm_ecommerce.domain.ItemCartDomain;
import com.example.prm_ecommerce.domain.OrderDetailDomain;
import com.example.prm_ecommerce.domain.OrderDomain;
import com.example.prm_ecommerce.domain.ProductDomain;

import org.json.JSONObject;

import java.io.IOException;
import java.text.NumberFormat;
import java.time.LocalDate;
import java.time.ZoneId;
import java.util.ArrayList;
import java.util.Currency;
import java.util.Date;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.HttpException;
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
    private IProductService ProductService;
    private IOrderService OrderService;
    private IOrderDetailService OrderDetailService;
    private IDeliveryService DeliveryService;

    ImageView ivAddAddress;
    Button btnOrder;

    TextView tvAddress;
    TextView tvDeliveryFee;

    String address;
    String deliveryFee;
    String totalString;
    int delivery = 0;

    OrderDomain order;
    OrderDetailDomain orderDetailDomain;
    List<ProductDomain> productDomainList = new ArrayList<>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        CartService = CartRepository.getCartService();
        ProductService = ProductRepository.getProductService();
        OrderService = OrderRepository.getService();
        OrderDetailService = OrderDetailRepository.getService();

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
//                    JSONObject data = orderApi.createOrder(totalString);
                    JSONObject data = orderApi.createOrder("1000");
                    Log.d("CreateOrder", "Response Data: " + data.toString());
                    String code = data.getString("return_code");

                    if (code.equals("1")) {
                        String token = data.getString("zp_trans_token");
                        ZaloPaySDK.getInstance().payOrder( CartActivity.this, token, "demozpdk://app", new PayOrderListener() {
                            @Override
                            public void onPaymentSucceeded(String s, String s1, String s2) {
//                                deleteCartByUserId();
                                getProductList();
//                                createOrder();
                                createOrderDetail();
                                createDelivery();
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

        if (getAddress!=null && getFee!=null){
            address = getAddress;
            delivery = Integer.valueOf(getFee);
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
                if(response.body() != null && response.body().getProducts() != null){
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
                if(response.body() != null && response.body().getProducts() != null){
                    for (ItemInCartModel product : response.body().getProducts()) {
                        items.add(new ItemCartDomain(
                                product.getProduct().get_id(),
                                product.getProduct().getName(),
                                product.getProduct().getImage(),
                                product.getQuantity(),
                                product.getProduct().getPrice()));
                    }
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
        int itemTotal = managementCart.getTotalFee();
        int total = Math.round(itemTotal + delivery);

        binding.tvDeliveryFee.setText(String.valueOf(delivery));
        binding.tvSubtotal.setText(String.valueOf(itemTotal));
        binding.tvTotal.setText(String.valueOf(total));

        totalString = String.valueOf(total);
    }

    private void setVariable() {
        binding.backBtn.setOnClickListener(v -> finish());
    }

    private void deleteCartByUserId(){
        Call<CartDomain> call = CartService.deleteCartByUserId(userId);
        call.enqueue(new Callback<CartDomain>() {
            @Override
            public void onResponse(Call<CartDomain> call, Response<CartDomain> response) {
                if (response.isSuccessful()) {

                    managementCart.clear();
                    Log.d("ResponseThu", response.toString() );

                    Toast.makeText(CartActivity.this, "Cart deleted successfully", Toast.LENGTH_SHORT).show();
                } else {
                    // Hiển thị mã lỗi nếu không thành công
                    Toast.makeText(CartActivity.this, "Failed Error code: " + response.code(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<CartDomain> call, Throwable throwable) {
//                Toast.makeText(CartActivity.this, "Fail delete cart ", Toast.LENGTH_SHORT).show();
//                Log.d("Error", throwable.getMessage());
                // Ghi lại thông điệp lỗi
                Log.e("API Error", "Error message: " + throwable.getMessage());

                // Nếu throwable là một IOException, bạn có thể muốn ghi lại nguyên nhân
                if (throwable instanceof IOException) {
                    Log.e("API Error", "Network Error: " + throwable.getMessage());
                }

                // Hiển thị một thông báo cho người dùng
                Toast.makeText(CartActivity.this, "Fail delete cart: " + throwable.getMessage(), Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void getProductList() {
        int productCount = managementCart.getListCart().size();
        int[] loadedCount = {0}; // Biến đếm sản phẩm đã tải

        for (ItemCartDomain item : managementCart.getListCart()) {
            Call<ProductDomain> call = ProductService.getProductById(item.getId());
            call.enqueue(new Callback<ProductDomain>() {
                @Override
                public void onResponse(Call<ProductDomain> call, Response<ProductDomain> response) {
                    productDomainList.add(response.body());
                    loadedCount[0]++;

                    // Nếu tất cả sản phẩm đã được tải
                    if (loadedCount[0] == productCount) {
                        createOrder(); // Gọi hàm tạo đơn hàng
                    }
                }

                @Override
                public void onFailure(Call<ProductDomain> call, Throwable throwable) {
                    // Xử lý lỗi nếu có
                }
            });
        }
    }

    private void createOrder(){
        Date currentDate = Date.from(LocalDate.now().atStartOfDay(ZoneId.systemDefault()).toInstant());

        final OrderDomain[] order = {new OrderDomain(
                null,      // cart
                currentDate,           // date
                false,                // isDeleted
                Integer.valueOf(binding.tvSubtotal.getText().toString()),             // priceBeforeShip
                productDomainList,          // products
                "pending",            // status
                Integer.valueOf(binding.tvTotal.getText().toString()),             // totalPrice
                userId,     // user
                null        // voucher
        )};

        Call<OrderDomain> call = OrderService.create(order[0]);
        call.enqueue(new Callback<OrderDomain>() {
            @Override
            public void onResponse(Call<OrderDomain> call, Response<OrderDomain> response) {
                order[0] = response.body();
                Toast.makeText(CartActivity.this, "Create order success", Toast.LENGTH_SHORT).show();
            }

            @Override
            public void onFailure(Call<OrderDomain> call, Throwable throwable) {

            }
        });
    }

    private void createOrderDetail(){
        for(ItemCartDomain item: managementCart.getListCart()){
            Call<OrderDetailDomain> call = OrderDetailService.create(new OrderDetailDomain(order.get_id(), item.getId(),
                                                                item.getNumberInCart(), ((int) item.getPrice())*item.getNumberInCart(),
                                                                (int)item.getPrice()));
        }
    }

    private void createDelivery(){
        DeliveryDomain deliveryDomain = new DeliveryDomain(order.get_id(),Integer.valueOf(tvDeliveryFee.getText().toString()),
                                                            tvAddress.getText().toString(), userId);

        Call<DeliveryDomain> call = DeliveryService.create(deliveryDomain);
        call.enqueue(new Callback<DeliveryDomain>() {
            @Override
            public void onResponse(Call<DeliveryDomain> call, Response<DeliveryDomain> response) {
                Toast.makeText(CartActivity.this, "Create delivery success", Toast.LENGTH_SHORT).show();
            }

            @Override
            public void onFailure(Call<DeliveryDomain> call, Throwable throwable) {

            }
        });
    }

    @Override
    protected void onNewIntent(@NonNull Intent intent) {
        super.onNewIntent(intent);
        ZaloPaySDK.getInstance().onResult(intent);

        setIntent(intent); // Cập nhật Intent mới
        setAddressAndDeliveryFee();
    }
}