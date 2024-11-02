package com.example.prm_ecommerce;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.media.RingtoneManager;
import android.os.Handler;
import android.os.Looper;
import android.widget.Toast;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.core.app.NotificationCompat;
import com.bumptech.glide.Glide;
import com.bumptech.glide.request.target.CustomTarget;
import com.example.prm_ecommerce.Activity.MainActivity;
import me.pushy.sdk.Pushy;
public class PushReceiver extends BroadcastReceiver {
    @Override
    public void onReceive(Context context, Intent intent) {
        String notificationTitle = intent.getStringExtra("title") != null ?
                intent.getStringExtra("title") :
                context.getPackageManager().getApplicationLabel(context.getApplicationInfo()).toString();
        String notificationText = intent.getStringExtra("message") != null ?
                intent.getStringExtra("message") : "Test notification";
        String imageUrl = intent.getStringExtra("imageUrl");
        // Create pending intent for notification click
        PendingIntent pendingIntent;
        if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.S) {
            pendingIntent = PendingIntent.getActivity(
                    context, 0,
                    new Intent(context, MainActivity.class),
                    PendingIntent.FLAG_MUTABLE
            );
        } else {
            pendingIntent = PendingIntent.getActivity(
                    context, 0,
                    new Intent(context, MainActivity.class),
                    PendingIntent.FLAG_UPDATE_CURRENT
            );
        }
        // Build the basic notification
        NotificationCompat.Builder builder = new NotificationCompat.Builder(context)
                .setAutoCancel(true)
                .setSmallIcon(android.R.drawable.ic_dialog_info)
                .setContentTitle(notificationTitle)
                .setContentText(notificationText)
                .setLights(Color.RED, 1000, 1000)
                .setVibrate(new long[]{0, 400, 250, 400})
                .setSound(RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION))
                .setContentIntent(pendingIntent);
        // Set the notification channel
        Pushy.setNotificationChannel(builder, context);
        // Create unique notification ID
        final int notificationId = (int) (Math.random() * 100000);
        if (imageUrl != null && !imageUrl.isEmpty()) {
            // Load image using Glide
            Glide.with(context.getApplicationContext())
                    .asBitmap()
                    .load(imageUrl)
                    .into(new CustomTarget<Bitmap>() {
                        @Override
                        public void onResourceReady(@NonNull Bitmap bitmap,
                                                    @Nullable com.bumptech.glide.request.transition.Transition<? super Bitmap> transition) {
                            // Add the image to the notification
                            NotificationCompat.BigPictureStyle style = new NotificationCompat.BigPictureStyle()
                                    .bigPicture(bitmap)
                                    .setSummaryText(notificationText);  // Show text when expanded
                            builder.setStyle(style);
                            // Show the notification with image
                            NotificationManager notificationManager =
                                    (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);
                            notificationManager.notify(notificationId, builder.build());
                        }
                        @Override
                        public void onLoadFailed(@Nullable Drawable errorDrawable) {
                            // Show notification without image if loading fails
                            NotificationManager notificationManager =
                                    (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);
                            notificationManager.notify(notificationId, builder.build());
                        }
                        @Override
                        public void onLoadCleared(@Nullable Drawable placeholder) {
                            // Not needed for this implementation
                        }
                    });
        } else {
            // Show notification without image if no URL provided
            NotificationManager notificationManager =
                    (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);
            notificationManager.notify(notificationId, builder.build());
        }
    }
}