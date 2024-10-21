package com.example.prm_ecommerce.domain;

public class ImageDomain {
    private String _id;
    private String product;
    private String imageUrl;

    public ImageDomain(String product, String imageUrl) {
        this.product = product;
        this.imageUrl = imageUrl;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public String getProduct() {
        return product;
    }

    public void setProduct(String product) {
        this.product = product;
    }

    public String getImageUrl() {
        return imageUrl;
    }

    public void setImageUrl(String imageUrl) {
        this.imageUrl = imageUrl;
    }
}
