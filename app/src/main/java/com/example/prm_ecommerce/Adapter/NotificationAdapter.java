package com.example.prm_ecommerce.Adapter;

import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.example.prm_ecommerce.API.Interface.INotificationService;
import com.example.prm_ecommerce.API.Repository.NotificationRepository;
import com.example.prm_ecommerce.Activity.DetailActivity;
import com.example.prm_ecommerce.Helper.ManagementCart;
import com.example.prm_ecommerce.databinding.ViewholderNotificationBinding;
import com.example.prm_ecommerce.databinding.ViewholderWishlistBinding;
import com.example.prm_ecommerce.domain.NotificationDomain;
import com.example.prm_ecommerce.domain.ProductDomain;

import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Currency;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class NotificationAdapter extends RecyclerView.Adapter<NotificationAdapter.Viewholder> {
    ArrayList<NotificationDomain> item;
    Context context;
    INotificationService NotificationService;

    public NotificationAdapter(ArrayList<NotificationDomain> item ) {
        this.item = item;
        NotificationService = NotificationRepository.getNoticationService();
    }
    @NonNull
    @Override
    public NotificationAdapter.Viewholder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        ViewholderNotificationBinding binding = ViewholderNotificationBinding.inflate(LayoutInflater.from(parent.getContext()), parent, false);
        context = parent.getContext();
        return new Viewholder(binding);
    }

    @Override
    public void onBindViewHolder(@NonNull NotificationAdapter.Viewholder holder, int position) {
        int i = position;
        NumberFormat format = NumberFormat.getCurrencyInstance();
        format.setMaximumFractionDigits(0);
        format.setCurrency(Currency.getInstance("VND"));
        NotificationDomain domain = item.get(position);
        holder.binding.tvName.setText(domain.getTitle());
        holder.binding.tvDescription.setText(domain.getMessage());
        holder.binding.tvIsSeen.setText(domain.getIsSeen() ? "" : "New");
        holder.binding.notificationLayout.setBackgroundColor(domain.getIsSeen() ? Color.parseColor("#EBEBEBFF") : Color.parseColor("#DDDDDD"));
        Glide.with(context)
                .load(domain.getImageUrl())
                .centerCrop()
                .into(holder.binding.pic);
        holder.itemView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Call<NotificationDomain> call = NotificationService.updateSeenMessage(domain.get_id());
                call.enqueue(new Callback<NotificationDomain>() {
                    @Override
                    public void onResponse(Call<NotificationDomain> call, Response<NotificationDomain> response) {
                        Toast.makeText(context, "Update seen", Toast.LENGTH_SHORT).show();
                    }

                    @Override
                    public void onFailure(Call<NotificationDomain> call, Throwable throwable) {

                    }
                });
            }
        });
//        holder.binding.tvName.setText(item.get(position).getName());
//        holder.binding.tvDescription.setText(item.get(position).getDescription());
//        holder.binding.tvTotalEachItem.setText(format.format(item.get(position).getPrice()));
//        Glide.with(context)
//                .load(item.get(i).getImage().get(0).getImageUrl())
//                .centerCrop()
//                .into(holder.binding.pic);
//        holder.itemView.setOnClickListener(v -> {
//            Intent intent = new Intent(context, DetailActivity.class);
//            intent.putExtra("object", item.get(position).get_id());
//            context.startActivity(intent);
//        });

    }

    @Override
    public int getItemCount() {
        return item.size();
    }

    public class Viewholder extends RecyclerView.ViewHolder {
        ViewholderNotificationBinding binding; // Khai báo binding tại đây

        public Viewholder(ViewholderNotificationBinding binding) {
            super(binding.getRoot());
            this.binding = binding; // Gán giá trị binding vào thuộc tính
        }
    }
}
