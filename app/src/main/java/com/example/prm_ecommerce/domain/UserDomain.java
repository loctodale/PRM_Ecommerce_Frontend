package com.example.prm_ecommerce.domain;

import java.util.List;

public class UserDomain {
    private String _id;
    private String email;
    private String username;
    private String password;
    private String name;
    private String address;
    private String phone;
    private String googleId;
    private List<ProductDomain> wishList;

    public UserDomain(String email, String username, String password, String name, String address, String phone, String googleId, List<ProductDomain> wishList) {
        this.email = email;
        this.username = username;
        this.password = password;
        this.name = name;
        this.address = address;
        this.phone = phone;
        this.googleId = googleId;
        this.wishList = wishList;
    }
    public UserDomain(String googleId,String email,String name, String phone){
        this.email = email;
        this.name = name;
        this.phone = phone;
        this.googleId = googleId;
    }
    public UserDomain(){

    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

    public String getGoogleId() {
        return googleId;
    }

    public void setGoogleId(String googleId) {
        this.googleId = googleId;
    }

    public List<ProductDomain> getWishList() {
        return wishList;
    }

    public void setWishList(List<ProductDomain> wishList) {
        this.wishList = wishList;
    }
}
