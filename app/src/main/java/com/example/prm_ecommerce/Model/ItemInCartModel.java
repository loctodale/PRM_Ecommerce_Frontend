package com.example.prm_ecommerce.Model;

import com.example.prm_ecommerce.domain.ProductDomain;

import java.util.List;

public class ItemInCartModel {
    private String _id;
    private ProductDomain product;
    private int quantity;

    public ItemInCartModel(ProductDomain product, int quantity) {
        this.product = product;
        this.quantity = quantity;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public ProductDomain getProduct() {
        return product;
    }

    public void setProduct(ProductDomain product) {
        this.product = product;
    }

    public int getQuantity() {
        return quantity;
    }

    public void setQuantity(int quantity) {
        this.quantity = quantity;
    }
}
