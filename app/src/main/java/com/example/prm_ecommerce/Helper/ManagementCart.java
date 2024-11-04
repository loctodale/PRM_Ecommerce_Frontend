package com.example.prm_ecommerce.Helper;

import android.content.Context;
import android.widget.Toast;

import com.example.prm_ecommerce.API.Interface.ICartService;
import com.example.prm_ecommerce.API.Repository.CartRepository;
import com.example.prm_ecommerce.CustomToast;
import com.example.prm_ecommerce.Model.RequestAddProductToCartModel;
import com.example.prm_ecommerce.domain.CartDomain;
import com.example.prm_ecommerce.domain.ItemCartDomain;

import java.util.ArrayList;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ManagementCart {
    private Context context;
    private TinyDB tinyDB;
    private ICartService CartService;
    public ManagementCart(Context context) {
        this.context = context;
        this.tinyDB=new TinyDB(context);
        this.CartService = CartRepository.getCartService();
    }

    public void clear(){
        tinyDB.clear();
    }

    public void insertItem(ItemCartDomain item) {
        ArrayList<ItemCartDomain> listpop = getListCart();
        boolean existAlready = false;
        int n = 0;
        for (int i = 0; i < listpop.size(); i++) {
            if (listpop.get(i).getName().equals(item.getName())) {
                existAlready = true;
                n = i;
                break;
            }
        }
        if(existAlready){
            listpop.get(n).setNumberInCart(item.getNumberInCart());
        }else{
            listpop.add(item);
        }
        tinyDB.putListObject("CartList",listpop);
//        Toast.makeText(context, "Added to your Cart", Toast.LENGTH_SHORT).show();
    }

    public ArrayList<ItemCartDomain> getListCart() {
        return tinyDB.getListObject("CartList");
    }

    public int getTotalFee(){
        ArrayList<ItemCartDomain> listItem=getListCart();
        double fee=0;
        for (int i = 0; i < listItem.size(); i++) {
            fee=fee+(listItem.get(i).getPrice()*listItem.get(i).getNumberInCart());
        }
        return (int)fee;
    }

    public void minusNumberItem(ArrayList<ItemCartDomain> listItem,int position,ChangeNumberItemsListener changeNumberItemsListener){
        String userId = "6718be16b762285e2490aae2";

        RequestAddProductToCartModel request = new RequestAddProductToCartModel(userId, listItem.get(position).getId(), 1);

        Call<CartDomain> call = CartService.removeQuantityInCart(request);
        call.enqueue(new Callback<CartDomain>() {
            @Override
            public void onResponse(Call<CartDomain> call, Response<CartDomain> response) {
                CustomToast.makeText(context, "Remove from cart success", CustomToast.LENGTH_LONG, CustomToast.SUCCESS,true).show();
            }

            @Override
            public void onFailure(Call<CartDomain> call, Throwable throwable) {
                CustomToast.makeText(context, throwable.getMessage(), CustomToast.LENGTH_LONG, CustomToast.ERROR,true).show();
            }
        });
        if(listItem.get(position).getNumberInCart()==1){

            listItem.remove(position);
        }else{
            listItem.get(position).setNumberInCart(listItem.get(position).getNumberInCart()-1);
        }
        tinyDB.putListObject("CartList",listItem);
        changeNumberItemsListener.change();
    }
    public void plusNumberItem(ArrayList<ItemCartDomain> listItem,int position,ChangeNumberItemsListener changeNumberItemsListener){
        String userId = "6718be16b762285e2490aae2";

        RequestAddProductToCartModel request = new RequestAddProductToCartModel(userId, listItem.get(position).getId(), 1);

        Call<CartDomain> call = CartService.addProductToCart(request);
        call.enqueue(new Callback<CartDomain>() {
            @Override
            public void onResponse(Call<CartDomain> call, Response<CartDomain> response) {
                CustomToast.makeText(context, "Add to cart success", CustomToast.LENGTH_LONG, CustomToast.SUCCESS,true).show();
            }

            @Override
            public void onFailure(Call<CartDomain> call, Throwable throwable) {
                CustomToast.makeText(context, "Error when add to cart", CustomToast.LENGTH_LONG, CustomToast.ERROR,true).show();
            }
        });
        listItem.get(position).setNumberInCart(listItem.get(position).getNumberInCart()+1);
        tinyDB.putListObject("CartList",listItem);
        changeNumberItemsListener.change();
    }
}
