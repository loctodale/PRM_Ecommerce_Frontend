package com.example.prm_ecommerce.Activity;

import android.content.SharedPreferences;

import android.os.Bundle;
import android.util.Log;

import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.prm_ecommerce.API.Interface.IUserService;
import com.example.prm_ecommerce.API.Repository.UserRepository;
import com.example.prm_ecommerce.Model.LoginSession;
import com.example.prm_ecommerce.R;

import com.example.prm_ecommerce.domain.UserDomain;


import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class UserProfileActivity extends AppCompatActivity {
     EditText edEmail,edName,edPhone,edAddress;
    private IUserService USerService;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_user_profile);

        USerService = UserRepository.getUserService();
        edName = findViewById(R.id.textView_show_full_name);
        edPhone = findViewById(R.id.textView_show_mobile);
        edEmail = findViewById(R.id.textView_show_email);
        edAddress = findViewById(R.id.textView_address);
        getUserById();

    }

    private void getUserById(){
       /* SharedPreferences sharedPreferences = getSharedPreferences("user_data", MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPreferences.edit();
        String userId = sharedPreferences.getString("user_id", null);
*/
        String userid = LoginSession.userId;
        Call<UserDomain> call = USerService.getUserById(userid);
        call.enqueue(new Callback<UserDomain>(){

            @Override
            public void onResponse(Call<UserDomain> call, Response<UserDomain> response) {
                if (response.isSuccessful() && response.body() != null) {
                    // Gán giá trị vào các EditText
                    edName.setText(response.body().getName());
                    edPhone.setText(response.body().getPhone());
                    edEmail.setText(response.body().getEmail());
                    edAddress.setText(response.body().getAddress());
                } else {
                    // Xử lý khi phản hồi không thành công
                    Toast.makeText(UserProfileActivity.this, "Response unsuccessful or empty body" , Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<UserDomain> call, Throwable throwable) {
                Toast.makeText(UserProfileActivity.this, "Response UserId unsuccessful or empty body" , Toast.LENGTH_SHORT).show();
            }
        });
    }

    public void update() {
        SharedPreferences sharedPreferences = getSharedPreferences("user_data", MODE_PRIVATE);
        String userId = sharedPreferences.getString("user_id", null);

        if (userId != null) {
            // Lấy dữ liệu từ các EditText để tạo đối tượng UserDomain cập nhật
            String name = edName.getText().toString();
            String phone = edPhone.getText().toString();
            String email = edEmail.getText().toString();
            String address = edAddress.getText().toString();

            // Tạo đối tượng UserDomain với dữ liệu mới
            UserDomain updatedUser = new UserDomain();
            updatedUser.setName(name);
            updatedUser.setPhone(phone);
            updatedUser.setEmail(email);
            updatedUser.setAddress(address);

            try {
                Call<UserDomain> call = USerService.updateUser(userId, updatedUser);
                call.enqueue(new Callback<UserDomain>() {
                    @Override
                    public void onResponse(Call<UserDomain> call, Response<UserDomain> response) {
                        if (response.body() != null) {
                            Toast.makeText(UserProfileActivity.this, "Save sucess", Toast.LENGTH_SHORT).show();
                            getUserById();
                        }
                    }

                    @Override
                    public void onFailure(Call<UserDomain> call, Throwable throwable) {
                        Toast.makeText(UserProfileActivity.this, "save fail", Toast.LENGTH_SHORT).show();
                        Log.w("MyTag", "requestFailed", throwable);
                    }
                });
            } catch (Exception ex) {
                Log.w("MyTag", "requestFailed");
            }
        }
    }
}
