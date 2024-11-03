package com.example.prm_ecommerce.domain;

import java.util.Date;
import java.util.List;

public class OrderDomain {
    private String _id;
    private String user;
    private String cart;
    private String voucher;
    private String priceBeforeShip;
    private String totalPrice;
    private String status;
    private Date date;
    private boolean isDeleted;
    private List<ProductDomain> products;

    public OrderDomain(String cart, Date date, boolean isDeleted, String priceBeforeShip, List<ProductDomain> products, String status, String totalPrice, String user, String voucher) {
        this.cart = cart;
        this.date = date;
        this.isDeleted = isDeleted;
        this.priceBeforeShip = priceBeforeShip;
        this.products = products;
        this.status = status;
        this.totalPrice = totalPrice;
        this.user = user;
        this.voucher = voucher;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public String getCart() {
        return cart;
    }

    public void setCart(String cart) {
        this.cart = cart;
    }

    public Date getDate() {
        return date;
    }

    public void setDate(Date date) {
        this.date = date;
    }

    public boolean isDeleted() {
        return isDeleted;
    }

    public void setDeleted(boolean deleted) {
        isDeleted = deleted;
    }

    public String getPriceBeforeShip() {
        return priceBeforeShip;
    }

    public void setPriceBeforeShip(String priceBeforeShip) {
        this.priceBeforeShip = priceBeforeShip;
    }

    public List<ProductDomain> getProducts() {
        return products;
    }

    public void setProducts(List<ProductDomain> products) {
        this.products = products;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getTotalPrice() {
        return totalPrice;
    }

    public void setTotalPrice(String totalPrice) {
        this.totalPrice = totalPrice;
    }

    public String getUser() {
        return user;
    }

    public void setUser(String user) {
        this.user = user;
    }

    public String getVoucher() {
        return voucher;
    }

    public void setVoucher(String voucher) {
        this.voucher = voucher;
    }
}
