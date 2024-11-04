package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.prm_ecommerce.API.Interface.IUserService;
import com.example.prm_ecommerce.API.Repository.UserRepository;
import com.example.prm_ecommerce.Model.LoginSession;
import com.example.prm_ecommerce.Model.UserModel;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.domain.LoginRequest;
import com.example.prm_ecommerce.domain.ResponseFirebaseDomain;
import com.example.prm_ecommerce.domain.UserDomain;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.AuthResult;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LoginActivity extends AppCompatActivity {
    private FirebaseAuth auth;
    IUserService userService;
    TextView txtAccount, txtPassword;
    Button btnLogin;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        auth = FirebaseAuth.getInstance();
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        txtAccount = findViewById(R.id.edt_email_si);
        txtPassword = findViewById(R.id.edt_pass_si);
        btnLogin = findViewById(R.id.btn_login);
        TextView register = findViewById(R.id.createText);// Link to the layout file
        register.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(LoginActivity.this, RegisterActivity.class);
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
        if (auth == null) {
            auth = FirebaseAuth.getInstance();
        }
        auth.signInWithEmailAndPassword(txtEmail, txtPassword)
                .addOnCompleteListener(LoginActivity.this, new OnCompleteListener<AuthResult>() {
                    @Override
                    public void onComplete(@NonNull Task<AuthResult> task) {
                        if (task.isSuccessful()) {
                            Toast.makeText(LoginActivity.this, "Login Successful!", Toast.LENGTH_SHORT).show();

                            FirebaseUser firebaseUser = auth.getCurrentUser();
                            if (firebaseUser != null) {
                                String userId = firebaseUser.getUid();
                                Log.d("Firebase", "Code user_id: " + userId);
                                LoginFireBase(userId, callback); // Pass the callback to handle success in LoginFireBase
                            } else {
                                callback.onLoginResult(false);
                            }
                        } else {
                            try {
                                throw task.getException();
                            } catch (Exception ex) {
                                Toast.makeText(LoginActivity.this, ex.getMessage(), Toast.LENGTH_SHORT).show();
                            }
                            callback.onLoginResult(false);
                        }
                    }
                });
    }

    private void LoginFireBase(String googleId, LoginCallback callback) {
        userService = UserRepository.getUserService();
        LoginRequest loginRequest = new LoginRequest(googleId);
        Call<ResponseFirebaseDomain> call = userService.loginFirebase(loginRequest);

        call.enqueue(new Callback<ResponseFirebaseDomain>() {
            @Override
            public void onResponse(Call<ResponseFirebaseDomain> call, Response<ResponseFirebaseDomain> response) {
                if (response.isSuccessful() && response.body() != null) {
                    String userId = response.body().getUserId();
                    Log.d("LoginActivity", "LoginFirebase user_id: " + userId);

                    LoginSession.userId = userId;

                    // Save to SharedPreferences
                    SharedPreferences sharedPreferences = getSharedPreferences("user_data", MODE_PRIVATE);
                    SharedPreferences.Editor editor = sharedPreferences.edit();
                    editor.putString("user_id", userId);
                    editor.apply();

                    // Callback success after Retrofit call is successful
                    callback.onLoginResult(true);
                } else {
                    Toast.makeText(LoginActivity.this, "Login unsuccessful or empty body", Toast.LENGTH_SHORT).show();
                    callback.onLoginResult(false);
                }
            }

            @Override
            public void onFailure(Call<ResponseFirebaseDomain> call, Throwable throwable) {
                Toast.makeText(LoginActivity.this, "Network Error: " + throwable.getMessage(), Toast.LENGTH_SHORT).show();
                callback.onLoginResult(false);
            }
        });
    }


}
