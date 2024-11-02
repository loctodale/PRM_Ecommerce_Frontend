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
import com.example.prm_ecommerce.API.Interface.IUserService;
import com.example.prm_ecommerce.API.Repository.CartRepository;
import com.example.prm_ecommerce.API.Repository.ProductRepository;
import com.example.prm_ecommerce.API.Repository.UserRepository;
import com.example.prm_ecommerce.CustomToast;
import com.example.prm_ecommerce.Model.RequestAddProductToCartModel;
import com.example.prm_ecommerce.Model.RequestUpdateWishList;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.databinding.ActivityDetailBinding;
import com.example.prm_ecommerce.databinding.ActivityMainBinding;
import com.example.prm_ecommerce.domain.CartDomain;
import com.example.prm_ecommerce.domain.ProductDomain;
import com.example.prm_ecommerce.domain.UserDomain;

import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Currency;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class DetailActivity extends AppCompatActivity {
    private ActivityDetailBinding binding;
    IProductService ProductService;
    ICartService CartService;
    IUserService UserService;
    private String _id;
    int numberOrder = 1;
    Boolean isFav = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        ProductService = ProductRepository.getProductService();
        CartService = CartRepository.getCartService();
        UserService = UserRepository.getUserService();
        binding = ActivityDetailBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        _id = (String) getIntent().getSerializableExtra("object");

        getBundles();

        binding.backBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });
        binding.bookmark.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                RequestUpdateWishList request = new RequestUpdateWishList("6718be16b762285e2490aae2", _id);
                Call<UserDomain> call = UserService.toggleWishList(request);
                call.enqueue(new Callback<UserDomain>() {
                    @Override
                    public void onResponse(Call<UserDomain> call, Response<UserDomain> response) {
                        if(isFav) {
                            binding.bookmark.setImageResource(R.drawable.bookmark);
                            isFav = false;
                        } else {
                            binding.bookmark.setImageResource(R.drawable.bookmark2);
                            isFav = true;
                        }
                    }

                    @Override
                    public void onFailure(Call<UserDomain> call, Throwable throwable) {

                    }
                });
            }
        });
    }

    private void getBundles() {
        NumberFormat format = NumberFormat.getCurrencyInstance();
        format.setMaximumFractionDigits(0);
        format.setCurrency(Currency.getInstance("VND"));

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
                        Call<UserDomain> callUser = UserService.getUserById("6718be16b762285e2490aae2");
                        callUser.enqueue(new Callback<UserDomain>() {
                            @Override
                            public void onResponse(Call<UserDomain> call, Response<UserDomain> response) {
                                for (ProductDomain product : response.body().getWishList()) {
                                    if(product.get_id().equals(object.get_id())){
                                        isFav = true;
                                    }
                                }
                                if (isFav) {
                                    binding.bookmark.setImageResource(R.drawable.bookmark2);
                                } else {
                                    binding.bookmark.setImageResource(R.drawable.bookmark);
                                }
                            }

                            @Override
                            public void onFailure(Call<UserDomain> call, Throwable throwable) {

                            }
                        });

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
