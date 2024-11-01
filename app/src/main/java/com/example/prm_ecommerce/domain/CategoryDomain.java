package com.example.prm_ecommerce.domain;

public class CategoryDomain {
    private String _id;
    private String name;
    private String isDelete;

    public CategoryDomain(String name, String isDelete) {
        this.name = name;
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

    public String getIsDelete() {
        return isDelete;
    }

    public void setIsDelete(String isDelete) {
        this.isDelete = isDelete;
    }
}
