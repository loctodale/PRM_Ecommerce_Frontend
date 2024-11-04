package com.example.prm_ecommerce.Adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.prm_ecommerce.API.Interface.IDeliveryService;
import com.example.prm_ecommerce.Activity.MapDirectionActivity;
import com.example.prm_ecommerce.Activity.OpenStreetMapActivity;
import com.example.prm_ecommerce.Model.DeliveryModel;
import com.example.prm_ecommerce.databinding.ViewholderDeliveryBinding;
import com.example.prm_ecommerce.databinding.ViewholderNotificationBinding;

import java.util.ArrayList;

public class DeliveryAdapter extends RecyclerView.Adapter<DeliveryAdapter.Viewholder>{
    ArrayList<DeliveryModel> deliveryList;
    IDeliveryService deliveryService;
    Context context;

    public DeliveryAdapter(ArrayList<DeliveryModel> deliveryList) {
        this.deliveryList = deliveryList;
        this.deliveryService = deliveryService;
        this.context = context;
    }

    @NonNull
    @Override
    public DeliveryAdapter.Viewholder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        ViewholderDeliveryBinding binding = ViewholderDeliveryBinding.inflate(LayoutInflater.from(parent.getContext()), parent, false);
        context = parent.getContext();
        return new Viewholder(binding);
    }

    @Override
    public void onBindViewHolder(@NonNull Viewholder holder, int position) {
        holder.binding.tvName.setText(deliveryList.get(position).getOrder().getUser().getName());
        holder.binding.tvPhone.setText(deliveryList.get(position).getOrder().getUser().getPhone());
        holder.binding.tvLocation.setText(deliveryList.get(position).getShippingLocation());
        holder.binding.tvPrice.setText(deliveryList.get(position).getShippingFee());
        holder.binding.pic.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(context, OpenStreetMapActivity.class);
                context.startActivity(intent);
            }
        });
    }

    @Override
    public int getItemCount() {
        return deliveryList.size();
    }

    public class Viewholder extends RecyclerView.ViewHolder {
        ViewholderDeliveryBinding binding; // Khai báo binding tại đây

        public Viewholder(ViewholderDeliveryBinding binding) {
            super(binding.getRoot());
            this.binding = binding; // Gán giá trị binding vào thuộc tính
        }
    }
}
