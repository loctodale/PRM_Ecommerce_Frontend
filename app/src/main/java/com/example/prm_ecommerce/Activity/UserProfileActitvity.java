package com.example.prm_ecommerce.Activity;

import android.os.Bundle;
import android.widget.EditText;
import android.widget.TextView;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.prm_ecommerce.R;

public class UserProfileActitvity extends AppCompatActivity {
    TextView edEmail,edName,edPhone,edAddress;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_user_profile);

        edName = findViewById(R.id.textView_show_full_name);
        edPhone = findViewById(R.id.textView_show_mobile);
        edEmail = findViewById(R.id.textView_show_email);
        edAddress = findViewById(R.id.textView_address);


    }
}
