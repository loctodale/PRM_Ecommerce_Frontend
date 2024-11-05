package com.example.prm_ecommerce.Model;

public class RequestCreateOrderStackModel {
    private String userId;
    private String voucherId;
    private float priceBeforeShip;
    private float totalPrice;
    private String orderStatus;
    private String cartId;
    private String shippingLocation;
    private String latLocation;
    private String longLocation;

    public RequestCreateOrderStackModel(String userId, String voucherId, float priceBeforeShip, float totalPrice, String orderStatus, String cartId, String shippingLocation, String latLocation, String longLocation) {
        this.userId = userId;
        this.voucherId = voucherId;
        this.priceBeforeShip = priceBeforeShip;
        this.totalPrice = totalPrice;
        this.orderStatus = orderStatus;
        this.cartId = cartId;
        this.shippingLocation = shippingLocation;
        this.latLocation = latLocation;
        this.longLocation = longLocation;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getVoucherId() {
        return voucherId;
    }

    public void setVoucherId(String voucherId) {
        this.voucherId = voucherId;
    }

    public float getPriceBeforeShip() {
        return priceBeforeShip;
    }

    public void setPriceBeforeShip(float priceBeforeShip) {
        this.priceBeforeShip = priceBeforeShip;
    }

    public float getTotalPrice() {
        return totalPrice;
    }

    public void setTotalPrice(float totalPrice) {
        this.totalPrice = totalPrice;
    }

    public String getOrderStatus() {
        return orderStatus;
    }

    public void setOrderStatus(String orderStatus) {
        this.orderStatus = orderStatus;
    }

    public String getCartId() {
        return cartId;
    }

    public void setCartId(String cartId) {
        this.cartId = cartId;
    }

    public String getShippingLocation() {
        return shippingLocation;
    }

    public void setShippingLocation(String shippingLocation) {
        this.shippingLocation = shippingLocation;
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
