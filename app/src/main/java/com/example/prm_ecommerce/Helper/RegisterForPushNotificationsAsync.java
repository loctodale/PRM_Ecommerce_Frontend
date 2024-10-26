package com.example.prm_ecommerce.Helper;

import android.app.Activity;
import android.os.AsyncTask;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;
import android.widget.Toast;

import java.net.HttpURLConnection;
import java.net.URL;

import me.pushy.sdk.Pushy;

public class RegisterForPushNotificationsAsync extends AsyncTask<Void, Void, Object> {
    Activity mActivity;

    public RegisterForPushNotificationsAsync(Activity activity) {
        this.mActivity = activity;
    }

    protected Object doInBackground(Void... params) {
        try {
            // Register the device for notifications (replace MainActivity with your Activity class name)
            String deviceToken = Pushy.register(mActivity);
//            new Handler(Looper.getMainLooper()).post(() -> {
//                Toast.makeText(mActivity.getApplicationContext(), deviceToken, Toast.LENGTH_SHORT).show();
//            });
            // Registration succeeded, log token to logcat
            Log.d("Pushy", "Pushy device token: " + deviceToken);

            // Send the token to your backend server via an HTTP GET request
            URL url = new URL("http://10.0.2.2:8765/notification/register?userId=6718be16b762285e2490aae2&deviceToken=" + deviceToken);
            HttpURLConnection conn = (HttpURLConnection) url.openConnection();
            conn.setRequestMethod("GET");
            conn.connect();

            int responseCode = conn.getResponseCode();
            // Provide token to onPostExecute()
            return deviceToken;
        }
        catch (Exception exc) {
            // Registration failed, provide exception to onPostExecute()
            return exc;
        }
    }

    @Override
    protected void onPostExecute(Object result) {
        String message;

        // Registration failed?
        if (result instanceof Exception) {
            // Log to console
            Log.e("Pushy", result.toString());

            // Display error in alert
            message = ((Exception) result).getMessage();
        }
        else {
            message = "Pushy device token: " + result.toString() + "\n\n(copy from logcat)";
        }

        // Registration succeeded, display an alert with the device token
        new android.app.AlertDialog.Builder(this.mActivity)
                .setTitle("Pushy")
                .setMessage(message)
                .setPositiveButton(android.R.string.ok, null)
                .show();
    }
}
