package com.example.prm_ecommerce.Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.example.prm_ecommerce.API.Interface.ICartService;
import com.example.prm_ecommerce.API.Repository.CartRepository;
import com.example.prm_ecommerce.Activity.DetailActivity;
import com.example.prm_ecommerce.CustomToast;
import com.example.prm_ecommerce.Helper.ChangeNumberItemsListener;
import com.example.prm_ecommerce.Helper.ManagementCart;
import com.example.prm_ecommerce.Model.RequestAddProductToCartModel;
import com.example.prm_ecommerce.databinding.ViewholderCartBinding;
import com.example.prm_ecommerce.domain.CartDomain;
import com.example.prm_ecommerce.domain.ItemCartDomain;

import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Currency;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class CartAdapter extends RecyclerView.Adapter<CartAdapter.Viewholder> {
    ArrayList<ItemCartDomain> item;
    Context context;
    ChangeNumberItemsListener  changeNumberItemListener;
    ManagementCart managementCart;
    ICartService CartService;

    public CartAdapter(ArrayList<ItemCartDomain> item, ChangeNumberItemsListener changeNumberItemListener) {
        this.changeNumberItemListener = changeNumberItemListener;
        this.item = item;
        this.CartService = CartRepository.getCartService();
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
        holder.binding.tvPriceEachItem.setText(format.format(item.get(i).getPrice()));
        holder.binding.tvTotalEachItem.setText(format.format(item.get(i).getNumberInCart()*item.get(i).getPrice()));
        holder.binding.tvNumberItem.setText(String.valueOf(item.get(i).getNumberInCart()));

        Glide.with(context)
                .load(item.get(i).getImages().get(0).getImageUrl())
                .centerCrop()
                .into(holder.binding.pic);

        holder.binding.plusCartBtn.setOnClickListener(view -> managementCart.plusNumberItem(item, i, () -> {
            notifyDataSetChanged();

            changeNumberItemListener.change();
        }));

        holder.binding.minusCartBtn.setOnClickListener(view -> managementCart.minusNumberItem(item, i, () -> {
            changeNumberItemListener.change();
            notifyDataSetChanged();

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
