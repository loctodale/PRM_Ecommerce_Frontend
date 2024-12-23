package com.example.prm_ecommerce.Model;

public class DeliveryModel {
    private String _id;
    private OrderModel order;
    private String shipper;
    private String shippingLocation;
    private String shippingFee;
    private String status;
    private String latLocation;
    private String longLocation;

    public DeliveryModel(OrderModel order, String shipper, String shippingLocation, String shippingFee, String status, String latLocation, String longLocation) {
        this.order = order;
        this.shipper = shipper;
        this.shippingLocation = shippingLocation;
        this.shippingFee = shippingFee;
        this.status = status;
        this.latLocation = latLocation;
        this.longLocation = longLocation;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public OrderModel getOrder() {
        return order;
    }

    public void setOrder(OrderModel order) {
        this.order = order;
    }

    public String getShipper() {
        return shipper;
    }

    public void setShipper(String shipper) {
        this.shipper = shipper;
    }

    public String getShippingLocation() {
        return shippingLocation;
    }

    public void setShippingLocation(String shippingLocation) {
        this.shippingLocation = shippingLocation;
    }

    public String getShippingFee() {
        return shippingFee;
    }

    public void setShippingFee(String shippingFee) {
        this.shippingFee = shippingFee;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
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


