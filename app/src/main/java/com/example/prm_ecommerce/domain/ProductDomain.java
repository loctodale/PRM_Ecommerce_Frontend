package com.example.prm_ecommerce.domain;

import java.io.Serializable;
import java.util.List;

public class ProductDomain {
    private String _id;
    private String name;
    private float price;
    private BrandDomain brand;
    private CategoryDomain category;
    private String description;
    private int quantitySold;
    private String origin;
    private String status;
    private boolean isDelete;
    private List<ImageDomain> images;

    public ProductDomain(String name, float price, CategoryDomain category, String description, int quantitySold, String origin, String status, boolean isDelete) {
        this.name = name;
        this.price = price;
        this.category = category;
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

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public float getPrice() {
        return price;
    }

    public void setPrice(float price) {
        this.price = price;
    }

    public BrandDomain getBrand() {
        return brand;
    }

    public void setBrand(BrandDomain brand) {
        this.brand = brand;
    }

    public CategoryDomain getCategory() {
        return category;
    }

    public void setCategory(CategoryDomain category) {
        this.category = category;
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

    public List<ImageDomain> getImage() {
        return images;
    }

    public void setImage(List<ImageDomain> image) {
        this.images = image;
    }
}
