package com.example.prm_ecommerce.Model;

import android.app.Application;

public class LoginSession {
    // Biến static cho session token và user ID
    public static String sessionToken = null;
    public static String userId = null;

    // Có thể thêm các phương thức tiện ích nếu cần, ví dụ để xóa session
    public static void clearSession() {
        sessionToken = null;
        userId = null;
    }
}