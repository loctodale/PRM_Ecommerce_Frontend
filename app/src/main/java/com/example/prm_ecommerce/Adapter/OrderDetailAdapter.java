package com.example.prm_ecommerce.Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.example.prm_ecommerce.API.Interface.IProductService;
import com.example.prm_ecommerce.API.Repository.ProductRepository;
import com.example.prm_ecommerce.databinding.ViewholderOrderDetailBinding;
import com.example.prm_ecommerce.domain.OrderDetailDomain;
import com.example.prm_ecommerce.domain.ProductDomain;

import java.util.ArrayList;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class OrderDetailAdapter extends RecyclerView.Adapter<OrderDetailAdapter.Viewholder> {
    ArrayList<OrderDetailDomain> listOrderDetail;
    Context context;
    ViewholderOrderDetailBinding binding;
    private IProductService productService;
    private ProductDomain productDomain;

    public OrderDetailAdapter(ArrayList<OrderDetailDomain> listOrder) {
        this.listOrderDetail = listOrder;
    }

    @NonNull
    @Override
    public Viewholder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        binding = ViewholderOrderDetailBinding.inflate(LayoutInflater.from(parent.getContext()), parent, false);
        context = parent.getContext();
        return new OrderDetailAdapter.Viewholder(binding);
    }

    @Override
    public void onBindViewHolder(@NonNull Viewholder holder, int position) {
        int i = position;
        productService = ProductRepository.getProductService();

        Call<ProductDomain> call = productService.getProductById(listOrderDetail.get(i).get_id());
        call.enqueue(new Callback<ProductDomain>() {
            @Override
            public void onResponse(Call<ProductDomain> call, Response<ProductDomain> response) {
                productDomain = response.body();
            }

            @Override
            public void onFailure(Call<ProductDomain> call, Throwable throwable) {

            }
        });
        binding.tvName.setText(productDomain.getName());
        binding.tvPriceEachItem.setText(listOrderDetail.get(i).getUnitPrice());
        binding.tvTotalEachItem.setText(listOrderDetail.get(i).getUnitPrice()*listOrderDetail.get(i).getQuantity());
        binding.tvQuantity.setText(listOrderDetail.get(i).getQuantity());
        Glide.with(context)
                .load(productDomain.getImage().get(0).getImageUrl())
                .centerCrop()
                .into(binding.pic);
    }

    @Override
    public int getItemCount() {
        return listOrderDetail.size();
    }

    public class Viewholder extends RecyclerView.ViewHolder {

        public Viewholder(ViewholderOrderDetailBinding binding) {
            super(binding.getRoot());
        }
    }
}
