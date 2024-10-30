package com.example.prm_ecommerce.Adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.example.prm_ecommerce.API.Interface.ICartService;
import com.example.prm_ecommerce.API.Repository.CartRepository;
import com.example.prm_ecommerce.Activity.DetailActivity;
import com.example.prm_ecommerce.Helper.ChangeNumberItemsListener;
import com.example.prm_ecommerce.Helper.ManagementCart;
import com.example.prm_ecommerce.databinding.ViewholderCartBinding;
import com.example.prm_ecommerce.databinding.ViewholderWishlistBinding;
import com.example.prm_ecommerce.domain.ItemCartDomain;
import com.example.prm_ecommerce.domain.ProductDomain;
import com.example.prm_ecommerce.domain.UserDomain;

import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Currency;

public class WishlistAdapter extends RecyclerView.Adapter<WishlistAdapter.Viewholder> {
    ArrayList<ProductDomain> item;
    Context context;

    public WishlistAdapter(ArrayList<ProductDomain> item ) {
        this.item = item;
    }
    @NonNull
    @Override
    public WishlistAdapter.Viewholder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        ViewholderWishlistBinding binding = ViewholderWishlistBinding.inflate(LayoutInflater.from(parent.getContext()), parent, false);
        context = parent.getContext();
        return new Viewholder(binding);
    }

    @Override
    public void onBindViewHolder(@NonNull WishlistAdapter.Viewholder holder, int position) {
        int i = position;
        NumberFormat format = NumberFormat.getCurrencyInstance();
        format.setMaximumFractionDigits(0);
        format.setCurrency(Currency.getInstance("VND"));

        holder.binding.tvName.setText(item.get(position).getName());
        holder.binding.tvDescription.setText(item.get(position).getDescription());
        holder.binding.tvTotalEachItem.setText(format.format(item.get(position).getPrice()));
        Glide.with(context)
                .load(item.get(i).getImage().get(0).getImageUrl())
                .centerCrop()
                .into(holder.binding.pic);
        holder.itemView.setOnClickListener(v -> {
            Intent intent = new Intent(context, DetailActivity.class);
            intent.putExtra("object", item.get(position).get_id());
            context.startActivity(intent);
        });

    }

    @Override
    public int getItemCount() {
        return item.size();
    }

    public class Viewholder extends RecyclerView.ViewHolder {
        ViewholderWishlistBinding binding; // Khai báo binding tại đây

        public Viewholder(ViewholderWishlistBinding binding) {
            super(binding.getRoot());
            this.binding = binding; // Gán giá trị binding vào thuộc tính
        }
    }
}
