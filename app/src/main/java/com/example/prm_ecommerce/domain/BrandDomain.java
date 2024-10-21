package com.example.prm_ecommerce.domain;

public class BrandDomain {
    private String _id;
    private String name;

    public BrandDomain(String name) {
        this.name = name;
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
}
