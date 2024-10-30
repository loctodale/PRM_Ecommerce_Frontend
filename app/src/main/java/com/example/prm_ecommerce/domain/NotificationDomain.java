package com.example.prm_ecommerce.domain;

public class NotificationDomain {
    private String _id;
    private String user;
    private String title;
    private String message;
    private String imageUrl;
    private boolean isSeen;

    public NotificationDomain(String user, String title, String message, String imageUrl, boolean isSeen) {
        this.user = user;
        this.title = title;
        this.message = message;
        this.imageUrl = imageUrl;
        this.isSeen = isSeen;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public String getUser() {
        return user;
    }

    public void setUser(String user) {
        this.user = user;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getImageUrl() {
        return imageUrl;
    }

    public void setImageUrl(String imageUrl) {
        this.imageUrl = imageUrl;
    }

    public boolean getIsSeen() {
        return isSeen;
    }

    public void setIsSeen(boolean isSeen) {
        this.isSeen = isSeen;
    }
}
