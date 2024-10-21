package com.example.prm_ecommerce.Adapter;

import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.load.resource.bitmap.GranularRoundedCorners;
import com.example.prm_ecommerce.databinding.ViewholderPupListBinding;
import com.example.prm_ecommerce.domain.ProductDomain;
import com.example.prm_ecommerce.R;

import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Currency;

public class PopularAdapter extends RecyclerView.Adapter<PopularAdapter.Viewholder> {
    ArrayList<ProductDomain> item;
    Context context;
    ViewholderPupListBinding binding;

    public PopularAdapter(ArrayList<ProductDomain> item) {
        this.item = item;
    }

    @NonNull
    @Override

    public PopularAdapter.Viewholder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        binding = ViewholderPupListBinding.inflate(LayoutInflater.from(parent.getContext()), parent, false);
        context = parent.getContext();
        return new Viewholder(binding);
    }

    @Override
    public void onBindViewHolder(@NonNull PopularAdapter.Viewholder holder, int position) {
        NumberFormat format = NumberFormat.getCurrencyInstance();
        format.setMaximumFractionDigits(0);
        format.setCurrency(Currency.getInstance("VND"));

        binding.titleTxt.setText(item.get(position).getName());
        binding.feeTxt.setText(format.format(item.get(position).getPrice()));
        binding.scoreTxt.setText(item.get(position).getQuantitySold()+"");
        Toast.makeText(context, item.get(position).getPrice() + "", Toast.LENGTH_SHORT).show();
        Glide.with(context)
                .load(item.get(position).getImage().get(0).getImageUrl())
                .centerCrop()
                .into(binding.pic);

        holder.itemView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            }
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
