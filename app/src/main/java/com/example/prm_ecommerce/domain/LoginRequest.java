package com.example.prm_ecommerce.domain;

public class LoginRequest {
    private String googleId;

    public LoginRequest(String googleId) {
        this.googleId = googleId;
    }

    public String getGoogleId() {
        return googleId;
    }
}
