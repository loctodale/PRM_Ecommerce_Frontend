package com.example.prm_ecommerce.domain;

import java.util.List;

public class ItemCartDomain {
    private String id;
    private String name;
    private List<ImageDomain> images;
    private int numberInCart;
    private double price;

    public ItemCartDomain(String id, List<ImageDomain> images, String name, double price) {
        this.id = id;
        this.images = images;
        this.name = name;
        this.price = price;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public List<ImageDomain> getImages() {
        return images;
    }

    public void setImages(List<ImageDomain> images) {
        this.images = images;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getNumberInCart() {
        return numberInCart;
    }

    public void setNumberInCart(int numberInCart) {
        this.numberInCart = numberInCart;
    }

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }
}
