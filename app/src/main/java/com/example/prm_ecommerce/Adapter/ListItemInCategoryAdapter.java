package com.example.prm_ecommerce.Adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.example.prm_ecommerce.Activity.DetailActivity;
import com.example.prm_ecommerce.databinding.ViewholderPupListBinding;
import com.example.prm_ecommerce.domain.ProductDomain;

import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Currency;

public class ListItemInCategoryAdapter extends RecyclerView.Adapter<ListItemInCategoryAdapter.Viewholder> {
    ArrayList<ProductDomain> item;
    Context context;
    ViewholderPupListBinding binding;

    public ListItemInCategoryAdapter(ArrayList<ProductDomain> item) {
        this.item = item;
    }

    @NonNull
    @Override

    public ListItemInCategoryAdapter.Viewholder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        binding = ViewholderPupListBinding.inflate(LayoutInflater.from(parent.getContext()), parent, false);
        context = parent.getContext();
        return new Viewholder(binding);
    }

    @Override
    public void onBindViewHolder(@NonNull ListItemInCategoryAdapter.Viewholder holder, int position) {
        NumberFormat format = NumberFormat.getCurrencyInstance();
        format.setMaximumFractionDigits(0);
        format.setCurrency(Currency.getInstance("VND"));

        binding.titleTxt.setText(item.get(position).getName());
        binding.feeTxt.setText(format.format(item.get(position).getPrice()));
        binding.scoreTxt.setText(item.get(position).getQuantitySold()+"");
        Glide.with(context)
                .load(item.get(position).getImage().get(0).getImageUrl())
                .centerCrop()
                .into(binding.pic);

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

        public Viewholder(ViewholderPupListBinding binding) {
            super(binding.getRoot());
        }
    }
}
