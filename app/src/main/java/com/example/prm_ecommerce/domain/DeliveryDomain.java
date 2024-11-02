package com.example.prm_ecommerce.domain;

public class DeliveryDomain {
    private String _id;
    private String orderId;
    private String userId;
    private String shippingLocation;
    private int shippingFee;

    public DeliveryDomain(String orderId, int shippingFee, String shippingLocation, String userId) {
        this.orderId = orderId;
        this.shippingFee = shippingFee;
        this.shippingLocation = shippingLocation;
        this.userId = userId;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public String getOrderId() {
        return orderId;
    }

    public void setOrderId(String orderId) {
        this.orderId = orderId;
    }

    public int getShippingFee() {
        return shippingFee;
    }

    public void setShippingFee(int shippingFee) {
        this.shippingFee = shippingFee;
    }

    public String getShippingLocation() {
        return shippingLocation;
    }

    public void setShippingLocation(String shippingLocation) {
        this.shippingLocation = shippingLocation;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }
}
