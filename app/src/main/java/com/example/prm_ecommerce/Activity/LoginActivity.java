package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.prm_ecommerce.R;

public class LoginActivity extends AppCompatActivity {

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
           if(Login(txtAccount.getText().toString(), txtPassword.getText().toString())){
               Intent intent = new Intent(LoginActivity.this, MainActivity.class);
               startActivity(intent);

               //thieu gan api va du lieu add bvao trong local
           }
           else{
               Toast.makeText(LoginActivity.this, "Account or Password is wrong" , Toast.LENGTH_SHORT).show();
           }
            }
        });
    }
    private boolean Login(String txtEmaill , String txtPassword){
        return false;
    }

}
