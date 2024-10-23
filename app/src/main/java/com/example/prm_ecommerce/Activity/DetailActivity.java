package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.bumptech.glide.Glide;
import com.example.prm_ecommerce.API.Interface.ICartService;
import com.example.prm_ecommerce.API.Interface.IProductService;
import com.example.prm_ecommerce.API.Repository.CartRepository;
import com.example.prm_ecommerce.API.Repository.ProductRepository;
import com.example.prm_ecommerce.CustomToast;
import com.example.prm_ecommerce.Model.RequestAddProductToCartModel;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityDetailBinding;
import com.example.prm_ecommerce.databinding.ActivityMainBinding;
import com.example.prm_ecommerce.domain.CartDomain;
import com.example.prm_ecommerce.domain.ProductDomain;

import java.text.NumberFormat;
import java.util.Currency;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class DetailActivity extends AppCompatActivity {
    private ActivityDetailBinding binding;
    IProductService ProductService;
    ICartService CartService;
    private String _id;
    int numberOrder = 1;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        ProductService = ProductRepository.getProductService();
        CartService = CartRepository.getCartService();
        binding = ActivityDetailBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        getBundles();

        binding.backBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(DetailActivity.this, MainActivity.class);
                DetailActivity.this.startActivity(intent);
            }
        });
    }

    private void getBundles() {
        NumberFormat format = NumberFormat.getCurrencyInstance();
        format.setMaximumFractionDigits(0);
        format.setCurrency(Currency.getInstance("VND"));
        _id = (String) getIntent().getSerializableExtra("object");
        Call<ProductDomain> call = ProductService.getProductById(_id);
        call.enqueue(new Callback<ProductDomain>() {
            @Override
            public void onResponse(Call<ProductDomain> call, Response<ProductDomain> response) {
                if (response.isSuccessful()) {
                    ProductDomain object = response.body();
                    if (object != null) {
                        Glide.with(DetailActivity.this)
                                .load(object.getImage().get(0).getImageUrl())
                                .centerCrop()
                                .into(binding.itemPic);
                        binding.titleTxt.setText(object.getName());
                        binding.priceTxt.setText(format.format(object.getPrice()));
                        binding.descriptionTxt.setText(object.getDescription());
                        binding.addToCartBtn.setOnClickListener(new View.OnClickListener() {
                            @Override
                            public void onClick(View v) {
                                String userId = "6718be16b762285e2490aae2";

                                RequestAddProductToCartModel request = new RequestAddProductToCartModel(userId, _id, 1);

                                Call<CartDomain> call = CartService.addProductToCart(request);
                                call.enqueue(new Callback<CartDomain>() {
                                    @Override
                                    public void onResponse(Call<CartDomain> call, Response<CartDomain> response) {
                                        CustomToast.makeText(DetailActivity.this, "Add to cart success", CustomToast.LENGTH_LONG, CustomToast.SUCCESS,true).show();
                                    }

                                    @Override
                                    public void onFailure(Call<CartDomain> call, Throwable throwable) {
                                        CustomToast.makeText(DetailActivity.this, "Error when add to cart", CustomToast.LENGTH_LONG, CustomToast.ERROR,true).show();
                                    }
                                });
                            }
                        });
                    } else {
                        Toast.makeText(DetailActivity.this, "Response body is null", Toast.LENGTH_SHORT).show();
                        Log.e("Success", "Response body is null");
                    }
                } else {
                    Toast.makeText(DetailActivity.this, "Error: " + response.code(), Toast.LENGTH_SHORT).show();
                    Log.e("Loi", "Error: " + response.code() + " " + response.message());
                }
            }

            @Override
            public void onFailure(Call<ProductDomain> call, Throwable throwable) {
                Toast.makeText(DetailActivity.this, "Network error: " + throwable.getMessage(), Toast.LENGTH_SHORT).show();
                Log.e("Error", "Network error", throwable);
            }
        });
    }
}
