package com.example.prm_ecommerce.domain;

public class DeliveryDomain {
    private String _id;
    private String order;
    private String shipper;
    private String shippingLocation;
    private int shippingFee;
    private String latLocation;
    private String longLocation;
    private String status;

    public DeliveryDomain(String latLocation, String longLocation, String order, String shipper, int shippingFee, String shippingLocation, String status) {
        this.latLocation = latLocation;
        this.longLocation = longLocation;
        this.order = order;
        this.shipper = shipper;
        this.shippingFee = shippingFee;
        this.shippingLocation = shippingLocation;
        this.status = status;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
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

    public String getOrder() {
        return order;
    }

    public void setOrder(String order) {
        this.order = order;
    }

    public String getShipper() {
        return shipper;
    }

    public void setShipper(String shipper) {
        this.shipper = shipper;
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

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }
}
