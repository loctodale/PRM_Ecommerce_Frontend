package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.prm_ecommerce.API.Interface.IUserService;
import com.example.prm_ecommerce.Model.UserModel;
import com.example.prm_ecommerce.R;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.AuthResult;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;

public class LoginActivity extends AppCompatActivity {
    private FirebaseAuth auth;
    IUserService userService;
    TextView txtAccount,txtPassword;
    Button btnLogin;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        txtAccount = findViewById(R.id.edt_email_si);
        txtPassword = findViewById(R.id.edt_pass_si);
        btnLogin = findViewById(R.id.btn_login);
        TextView register = findViewById(R.id.createText);// Link to the layout file
        register.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(LoginActivity.this , RegisterActivity.class);
                startActivity(intent);
            }
        });

        btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Login(txtAccount.getText().toString(), txtPassword.getText().toString(), new LoginCallback() {
                    @Override
                    public void onLoginResult(boolean isSuccess) {
                        if (isSuccess) {
                            // Đăng nhập thành công, chuyển đến MainActivity
                            Intent intent = new Intent(LoginActivity.this, MainActivity.class);
                            startActivity(intent);
                            finish(); // Đóng LoginActivity
                        } else {
                            // Đăng nhập thất bại, hiển thị thông báo lỗi
                            Toast.makeText(LoginActivity.this, "Account or Password is wrong", Toast.LENGTH_SHORT).show();
                        }
                    }
                });
            }
        });
    }
    public interface LoginCallback {
        void onLoginResult(boolean isSuccess);
    }

    private void Login(String txtEmail, String txtPassword, LoginCallback callback) {
        auth.signInWithEmailAndPassword(txtEmail, txtPassword)
                .addOnCompleteListener(LoginActivity.this, new OnCompleteListener<AuthResult>() {
                    @Override
                    public void onComplete(@NonNull Task<AuthResult> task) {
                        if (task.isSuccessful()) {
                            Toast.makeText(LoginActivity.this, "Login Successful!", Toast.LENGTH_SHORT).show();

                            FirebaseUser firebaseUser = auth.getCurrentUser();
                            if (firebaseUser != null) {
                                String userId = firebaseUser.getUid();          // UID của người dùng
                                String userEmail = firebaseUser.getEmail();      // Email của người dùng

                                // Lưu vào SharedPreferences
                                SharedPreferences sharedPreferences = getSharedPreferences("user_data", MODE_PRIVATE);
                                SharedPreferences.Editor editor = sharedPreferences.edit();
                                editor.putString("user_id", userId);
                                editor.putString("user_email", userEmail);
                                editor.apply();
                            }

                            // Gọi lại callback với kết quả thành công
                            callback.onLoginResult(true);
                        } else {
                            try {
                                throw task.getException();
                            } catch (Exception ex) {
                                Toast.makeText(LoginActivity.this, ex.getMessage(), Toast.LENGTH_SHORT).show();
                            }
                            // Gọi lại callback với kết quả thất bại
                            callback.onLoginResult(false);
                        }
                    }
                });
    }



}
