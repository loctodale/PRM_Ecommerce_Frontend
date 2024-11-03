package com.example.prm_ecommerce.domain;

public class ChatMessage {
    private String userId;
    private String message;
    private long timestamp;
    private boolean isSystem;

    public ChatMessage() {}

    public ChatMessage(String userId, String message, long timestamp, boolean isSystem) {
        this.userId = userId;
        this.message = message;
        this.timestamp = timestamp;
        this.isSystem = isSystem;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public long getTimestamp() {
        return timestamp;
    }

    public void setTimestamp(long timestamp) {
        this.timestamp = timestamp;
    }

    public boolean isSystem() {
        return isSystem;
    }

    public void setSystem(boolean system) {
        isSystem = system;
    }
}
