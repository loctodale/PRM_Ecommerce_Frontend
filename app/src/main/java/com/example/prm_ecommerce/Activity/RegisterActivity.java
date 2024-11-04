package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.util.Patterns;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.prm_ecommerce.API.Interface.IUserService;
import com.example.prm_ecommerce.API.Repository.UserRepository;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.domain.UserDomain;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.AuthResult;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class RegisterActivity extends AppCompatActivity {

    IUserService userService;
    EditText edFullName,edEmail,edDob,edMobile,edPassword,edConfirmPwd;
    private ProgressBar progressBar;
    private RadioGroup radioGroupRegisterGender;
    private RadioButton radioButtonRegisterGenderSelected;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

//Api
        userService = UserRepository.getUserService();
        //edit text for information
        Toast.makeText(RegisterActivity.this, "You can register now", Toast.LENGTH_SHORT).show();
        progressBar = findViewById(R.id.progressBar);
        edFullName = findViewById(R.id.editText_register_full_name);
        edEmail = findViewById(R.id.editText_register_email);
        edDob = findViewById(R.id.editText_register_dob);
        edMobile = findViewById(R.id.editText_register_mobile);
        edPassword = findViewById(R.id.editText_register_password);
        edConfirmPwd=findViewById(R.id.editText_register_confirmpassword);

        //radioButton for Gender
        radioGroupRegisterGender = findViewById(R.id.radio_group_register_gender);
        radioGroupRegisterGender.clearCheck();
        Button buttonCancel = findViewById(R.id.button_cancel);
        buttonCancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(RegisterActivity.this, MainActivity.class);
                startActivity(intent);
                finish();
            }
        });
        Button buttonRegister = findViewById(R.id.button_register);
        buttonRegister.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                int selectedfGenderId = radioGroupRegisterGender.getCheckedRadioButtonId();
                radioButtonRegisterGenderSelected = findViewById(selectedfGenderId);

                //Obtain the entered data
                String textFullName = edFullName.getText().toString();
                String textEmail = edEmail.getText().toString();
                String textDob = edDob.getText().toString();
                String textMobile = edMobile.getText().toString();
                String textPwd = edPassword.getText().toString();
                String textConfirmPwd = edConfirmPwd.getText().toString();
                String textGender;

                if(TextUtils.isEmpty(textFullName)){
                    Toast.makeText(RegisterActivity.this , "Please enter your fullName ", Toast.LENGTH_SHORT).show();
                    edFullName.setError("Full name is required");
                    edFullName.requestFocus();
                }else if (TextUtils.isEmpty(textEmail)) {
                    Toast.makeText(RegisterActivity.this , "Please enter your email ", Toast.LENGTH_SHORT).show();
                    edEmail.setError("Email is required");
                    edEmail.requestFocus();
                } else if (!Patterns.EMAIL_ADDRESS.matcher(textEmail).matches()) {
                    Toast.makeText(RegisterActivity.this , "Please re-enter your email ", Toast.LENGTH_SHORT).show();
                    edEmail.setError("Valid email is required");
                    edEmail.requestFocus();
                }else if (TextUtils.isEmpty(textDob)) {
                    Toast.makeText(RegisterActivity.this, "Please enter your date of birth", Toast.LENGTH_SHORT).show();
                    edDob.setError("Date of birth is required");
                    edDob.requestFocus();
                } else if (TextUtils.isEmpty(textMobile)) {
                    Toast.makeText(RegisterActivity.this, "Please enter your mobile number", Toast.LENGTH_SHORT).show();
                    edMobile.setError("Mobile number is required");
                    edMobile.requestFocus();
                } else if (!Patterns.PHONE.matcher(textMobile).matches() || textMobile.length() < 10) {
                    Toast.makeText(RegisterActivity.this, "Please enter a valid mobile number", Toast.LENGTH_SHORT).show();
                    edMobile.setError("Valid mobile number is required");
                    edMobile.requestFocus();
                } else if (TextUtils.isEmpty(textPwd)) {
                    Toast.makeText(RegisterActivity.this, "Please enter your password", Toast.LENGTH_SHORT).show();
                    edPassword.setError("Password is required");
                    edPassword.requestFocus();
                } else if (textPwd.length() < 6) {
                    Toast.makeText(RegisterActivity.this, "Password must be at least 6 characters", Toast.LENGTH_SHORT).show();
                    edPassword.setError("Password must be at least 6 characters");
                    edPassword.requestFocus();
                } else if (TextUtils.isEmpty(textConfirmPwd)) {
                    Toast.makeText(RegisterActivity.this, "Please enter your password", Toast.LENGTH_SHORT).show();
                    edConfirmPwd.setError("Password is required");
                    edConfirmPwd.requestFocus();
                }else if (!textPwd.equals(textConfirmPwd)) {
                    Toast.makeText(RegisterActivity.this, "Please same your password", Toast.LENGTH_SHORT).show();
                    edConfirmPwd.setError("Password Confirmation is required");
                    edConfirmPwd.requestFocus();
                    //Clear the entered passwords
                    edConfirmPwd.clearComposingText();
                    edPassword.clearComposingText();
                }else{
                    textGender = radioButtonRegisterGenderSelected.getText().toString();
                    progressBar.setVisibility(View.VISIBLE);
                    registerUser(textFullName,textEmail,textDob,textMobile,textGender, textPwd);
                }
            }
        });
    }

    //Register User using the credentialts given
    private void registerUser(String fullname, String Email, String Dob , String Phone, String Gender, String textPwd){
        FirebaseAuth auth = FirebaseAuth.getInstance();
        auth.createUserWithEmailAndPassword(Email,textPwd).addOnCompleteListener(RegisterActivity.this,
                new OnCompleteListener<AuthResult>() {
                    @Override
                    public void onComplete(@NonNull Task<AuthResult> task) {
                        if(task.isSuccessful()){
                            Toast.makeText(RegisterActivity.this, "User registered successfully", Toast.LENGTH_SHORT).show();
                            FirebaseUser firebaseUser = auth.getCurrentUser();
                            //dang ky user firebase
                            String googleId = firebaseUser.getUid();
                            CreateUser( Email, Phone, fullname,googleId);
                            //Send Verification Email
                            firebaseUser.sendEmailVerification();
                            NavigateLogin();
                        }
                    }
                }
        );
    }

    private void NavigateLogin(){
        Intent intent = new Intent(RegisterActivity.this , LoginActivity.class);
        startActivity(intent);
    }

    public void CreateUser(String Email,String Phone,String name, String googleId){
         UserDomain userDomain = new UserDomain(googleId,Email,name,Phone);
         Call<UserDomain> call = userService.registerFirebase(userDomain);
         try {
             call.enqueue(new Callback<UserDomain>() {
                 @Override
                 public void onResponse(Call<UserDomain> call, Response<UserDomain> response) {
                     if(response.body() != null){
                         Toast.makeText(RegisterActivity.this, "Register sucess", Toast.LENGTH_SHORT).show();
                     }
                 }

                 @Override
                 public void onFailure(Call<UserDomain> call, Throwable throwable) {
                     Toast.makeText(RegisterActivity.this, "Register fail", Toast.LENGTH_SHORT).show();
                     Log.w("MyTag", "requestFailed", throwable);
                 }
             });
         }catch (Exception ex){
             Log.w("MyTag", "requestFailed");
         }
    }
}
