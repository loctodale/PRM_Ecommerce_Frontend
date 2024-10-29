package com.example.prm_ecommerce.domain;

import com.example.prm_ecommerce.Model.ItemInCartModel;

import java.util.Date;
import java.util.List;

public class CartDomain {
    private String _id;
    private String user;
    private Date date;
    private List<ItemInCartModel> products;

    public CartDomain(String user, Date date, List<ItemInCartModel> products) {
        this.user = user;
        this.date = date;
        this.products = products;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public String getUser() {
        return user;
    }

    public void setUser(String user) {
        this.user = user;
    }

    public Date getDate() {
        return date;
    }

    public void setDate(Date date) {
        this.date = date;
    }

    public List<ItemInCartModel> getProducts() {
        return products;
    }

    public void setProducts(List<ItemInCartModel> products) {
        this.products = products;
    }
}
