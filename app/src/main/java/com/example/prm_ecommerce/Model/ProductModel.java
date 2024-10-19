package com.example.prm_ecommerce.Model;
public class ProductModel {
    private String _id;
    private float price;
    private String brand;
    private String category;
    private String descriptiondescription;
    private String description;
    private int quantitySold;
    private String origin;
    private String status;
    private boolean isDelete;

    public ProductModel(String brand, float price, String category, String descriptiondescription, String description, int quantitySold, String origin, String status, boolean isDelete) {
        this.brand = brand;
        this.price = price;
        this.category = category;
        this.descriptiondescription = descriptiondescription;
        this.description = description;
        this.quantitySold = quantitySold;
        this.origin = origin;
        this.status = status;
        this.isDelete = isDelete;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public float getPrice() {
        return price;
    }

    public void setPrice(float price) {
        this.price = price;
    }

    public String getBrand() {
        return brand;
    }

    public void setBrand(String brand) {
        this.brand = brand;
    }

    public String getCategory() {
        return category;
    }

    public void setCategory(String category) {
        this.category = category;
    }

    public String getDescriptiondescription() {
        return descriptiondescription;
    }

    public void setDescriptiondescription(String descriptiondescription) {
        this.descriptiondescription = descriptiondescription;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public int getQuantitySold() {
        return quantitySold;
    }

    public void setQuantitySold(int quantitySold) {
        this.quantitySold = quantitySold;
    }

    public String getOrigin() {
        return origin;
    }

    public void setOrigin(String origin) {
        this.origin = origin;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public boolean isDelete() {
        return isDelete;
    }

    public void setDelete(boolean delete) {
        isDelete = delete;
    }
}
