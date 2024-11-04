package com.example.prm_ecommerce.Model;

public class OrderModel {
    private String _id;
    private UserModel user;
    private String voucher;
    private String priceBeforeShip;
    private String totalPrice;
    private String status;
    private String date;

    public OrderModel(UserModel user, String voucher, String priceBeforeShip, String totalPrice, String status, String date) {
        this.user = user;
        this.voucher = voucher;
        this.priceBeforeShip = priceBeforeShip;
        this.totalPrice = totalPrice;
        this.status = status;
        this.date = date;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public UserModel getUser() {
        return user;
    }

    public void setUser(UserModel user) {
        this.user = user;
    }

    public String getVoucher() {
        return voucher;
    }

    public void setVoucher(String voucher) {
        this.voucher = voucher;
    }

    public String getPriceBeforeShip() {
        return priceBeforeShip;
    }

    public void setPriceBeforeShip(String priceBeforeShip) {
        this.priceBeforeShip = priceBeforeShip;
    }

    public String getTotalPrice() {
        return totalPrice;
    }

    public void setTotalPrice(String totalPrice) {
        this.totalPrice = totalPrice;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }
}
