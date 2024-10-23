package com.example.prm_ecommerce.Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.example.prm_ecommerce.Helper.ChangeNumberItemsListener;
import com.example.prm_ecommerce.Helper.ManagementCart;
import com.example.prm_ecommerce.databinding.ViewholderCartBinding;
import com.example.prm_ecommerce.domain.ItemCartDomain;

import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Currency;

public class CartAdapter extends RecyclerView.Adapter<CartAdapter.Viewholder> {
    ArrayList<ItemCartDomain> item;
    Context context;
    ChangeNumberItemsListener  changeNumberItemListener;
    ManagementCart managementCart;

    public CartAdapter(ArrayList<ItemCartDomain> item, ChangeNumberItemsListener changeNumberItemListener) {
        this.changeNumberItemListener = changeNumberItemListener;
        this.item = item;
    }

    @NonNull
    @Override

    public CartAdapter.Viewholder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        ViewholderCartBinding binding = ViewholderCartBinding.inflate(LayoutInflater.from(parent.getContext()), parent, false);
        context = parent.getContext();
        managementCart = new ManagementCart(context);
        return new Viewholder(binding);
    }

    @Override
    public void onBindViewHolder(@NonNull CartAdapter.Viewholder holder, int position) {
        int i = position;
        NumberFormat format = NumberFormat.getCurrencyInstance();
        format.setMaximumFractionDigits(0);
        format.setCurrency(Currency.getInstance("VND"));

        holder.binding.tvName.setText(item.get(i).getName());
        holder.binding.tvPriceEachItem.setText(item.get(i).getPrice()+"VND");
        holder.binding.tvTotalEachItem.setText(Math.round(item.get(i).getNumberInCart()*item.get(i).getPrice())+"VND");
        holder.binding.tvNumberItem.setText(String.valueOf(item.get(i).getNumberInCart()));

        Toast.makeText(context, item.get(i).getPrice() + "", Toast.LENGTH_SHORT).show();
//        Glide.with(context)
//                .load(item.get(i).getImages().get(0).getImageUrl())
//                .centerCrop()
//                .into(binding.pic);

        holder.binding.plusCartBtn.setOnClickListener(view -> managementCart.plusNumberItem(item, i, () -> {
            notifyDataSetChanged();
            changeNumberItemListener.change();
        }));

        holder.binding.minusCartBtn.setOnClickListener(view -> managementCart.minusNumberItem(item, i, () -> {
            notifyDataSetChanged();
            changeNumberItemListener.change();
        }));
    }

    @Override
    public int getItemCount() {
        return item.size();
    }

    public class Viewholder extends RecyclerView.ViewHolder {
        ViewholderCartBinding binding; // Khai báo binding tại đây

        public Viewholder(ViewholderCartBinding binding) {
            super(binding.getRoot());
            this.binding = binding; // Gán giá trị binding vào thuộc tính
        }
    }
}
