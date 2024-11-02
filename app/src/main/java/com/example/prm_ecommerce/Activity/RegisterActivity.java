package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.os.Bundle;
import android.text.TextUtils;
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

import com.example.prm_ecommerce.R;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.AuthResult;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;

public class RegisterActivity extends AppCompatActivity {

    EditText edFullName,edEmail,edDob,edMobile,edPassword,edConfirmPwd;
    private ProgressBar progressBar;
    private RadioGroup radioGroupRegisterGender;
    private RadioButton radioButtonRegisterGenderSelected;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);


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

                            //Send Verification Email
                            firebaseUser.sendEmailVerification();

                       /*     //Open User Profile after successful registration
                            Intent intent = new Intent(RegisterActivity.this, UserProfileActitvity.class);
                            //To Prevent User returning  back to Register Activity on pressing back button after registration
                            intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_CLEAR_TASK | Intent.FLAG_ACTIVITY_NEW_TASK);
                            startActivity(intent);
                            finish(); // to close all activity*/
                        }
                    }
                }
        );
    }
}
