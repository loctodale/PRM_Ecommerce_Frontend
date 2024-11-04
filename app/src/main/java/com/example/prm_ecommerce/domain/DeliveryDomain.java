package com.example.prm_ecommerce.domain;

public class DeliveryDomain {
    private String _id;
    private String orderId;
    private String userId;
    private String shippingLocation;
    private int shippingFee;
    private String latLocation;
    private String longLocation;

    public DeliveryDomain(String orderId, String userId, String shippingLocation, int shippingFee, String latLocation, String longLocation) {
        this.orderId = orderId;
        this.userId = userId;
        this.shippingLocation = shippingLocation;
        this.shippingFee = shippingFee;
        this.latLocation = latLocation;
        this.longLocation = longLocation;
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

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getShippingLocation() {
        return shippingLocation;
    }

    public void setShippingLocation(String shippingLocation) {
        this.shippingLocation = shippingLocation;
    }

    public int getShippingFee() {
        return shippingFee;
    }

    public void setShippingFee(int shippingFee) {
        this.shippingFee = shippingFee;
    }

    public String getLatLocation() {
        return latLocation;
    }

    public void setLatLocation(String latLocation) {
        this.latLocation = latLocation;
    }

    public String getLongLocation() {
        return longLocation;
    }

    public void setLongLocation(String longLocation) {
        this.longLocation = longLocation;
    }
}
