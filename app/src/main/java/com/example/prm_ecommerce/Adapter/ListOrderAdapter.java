package com.example.prm_ecommerce.Adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.prm_ecommerce.Activity.DetailActivity;
import com.example.prm_ecommerce.Activity.OrderDetailActivity;
import com.example.prm_ecommerce.databinding.ViewholderOrderBinding;
import com.example.prm_ecommerce.domain.OrderDomain;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

public class ListOrderAdapter extends RecyclerView.Adapter<ListOrderAdapter.Viewholder>{
    ArrayList<OrderDomain> item;
    Context context;
    ViewholderOrderBinding binding;

    public ListOrderAdapter(ArrayList<OrderDomain> item) {
        this.item = item;
    }

    @NonNull
    @Override

    public ListOrderAdapter.Viewholder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        binding = ViewholderOrderBinding.inflate(LayoutInflater.from(parent.getContext()), parent, false);
        context = parent.getContext();
        return new ListOrderAdapter.Viewholder(binding);
    }

    @Override
    public void onBindViewHolder(@NonNull ListOrderAdapter.Viewholder holder, int position) {
        // Giả sử item.get(position).getDate() trả về kiểu Date
        Date date = item.get(position).getDate();
        SimpleDateFormat dateFormat = new SimpleDateFormat("dd/MM/yyyy");
        String formattedDate = dateFormat.format(date);

        binding.tvDate.setText(formattedDate);
        binding.tvStatus.setText(item.get(position).getStatus());
        binding.tvTotal.setText(item.get(position).getTotalPrice());
        binding.tvDate.setText(formattedDate);

        holder.itemView.setOnClickListener(v -> {
            Intent intent = new Intent(context, OrderDetailActivity.class);
            intent.putExtra("orderId", item.get(position).get_id());
            context.startActivity(intent);
        });
    }

    @Override
    public int getItemCount() {
        return item.size();
    }

    public class Viewholder extends RecyclerView.ViewHolder {
        public Viewholder(ViewholderOrderBinding binding) {
            super(binding.getRoot());
        }
    }
}
