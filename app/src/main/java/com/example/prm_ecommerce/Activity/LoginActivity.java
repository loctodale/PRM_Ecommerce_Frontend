package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.prm_ecommerce.API.Interface.IUserService;
import com.example.prm_ecommerce.Model.UserModel;
import com.example.prm_ecommerce.R;

public class LoginActivity extends AppCompatActivity {

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
      // call api ru nhap vao tk and mk to login login response ve id luu o local
// Get SharedPreferences
        SharedPreferences sharedPreferences = getSharedPreferences("user_data", MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPreferences.edit();

// Save the data

       // editor.putString("userId", responseUserId); // add more if needed
        editor.apply(); // apply changes


        ///////////////////////////////////////// Retrieving data
      /*  SharedPreferences sharedPreferences = getSharedPreferences("user_data", MODE_PRIVATE);
        String token = sharedPreferences.getString("token", null);
        String userId = sharedPreferences.getString("userId", null);*/

        //////////////////////////// Clear data when u log out the application
  /*      SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.clear();
        editor.apply();*/
        return false;



    }

}
