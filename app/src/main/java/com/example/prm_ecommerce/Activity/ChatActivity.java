package com.example.prm_ecommerce.Activity;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.prm_ecommerce.Adapter.ChatAdapter;
import com.example.prm_ecommerce.R;
import com.example.prm_ecommerce.domain.ChatMessage;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.QueryDocumentSnapshot;

import java.util.ArrayList;
import java.util.List;

public class ChatActivity extends AppCompatActivity {

    private RecyclerView recyclerView;
    private EditText messageInput;
    private ImageButton sendButton;
    private ChatAdapter chatAdapter;
    private List<ChatMessage> messageList;
    private FirebaseFirestore db;
    private String chatRoomId;
    private String userId;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_chat);
        SharedPreferences sharedPreferences = getSharedPreferences("user_data", MODE_PRIVATE);
        userId = sharedPreferences.getString("user_id", null);
        db = FirebaseFirestore.getInstance();
        chatRoomId = userId;

        recyclerView = findViewById(R.id.recyclerView);
        messageInput = findViewById(R.id.messageInput);
        sendButton = findViewById(R.id.sendButton);

        messageList = new ArrayList<>();
        chatAdapter = new ChatAdapter(messageList);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));
        recyclerView.setAdapter(chatAdapter);

        addWelcomeMessage();

        // Setup send button click listener
        sendButton.setOnClickListener(v -> sendMessage());

        // Listen for messages
        listenForMessages();

    }

    private void addWelcomeMessage() {
        ChatMessage welcomeMessage = new ChatMessage(
                "system",
                "Xin chào, tôi có thể giúp gì cho bạn (Tin nhắn này là tin nhắn tự động, chúng tôi sẽ phản hồi trong thời gian sớm nhất)",
                System.currentTimeMillis(),
                true
        );
        messageList.add(welcomeMessage);
        chatAdapter.notifyDataSetChanged();
        recyclerView.scrollToPosition(messageList.size() - 1);
    }

    private void sendMessage() {
        String messageText = messageInput.getText().toString().trim();
        if (messageText.isEmpty()) return;

        // Create message object
        ChatMessage message = new ChatMessage(
                FirebaseAuth.getInstance().getCurrentUser().getUid(),
                messageText,
                System.currentTimeMillis(),
                false
        );

        // Add to Firestore
        db.collection("chats")
                .document(chatRoomId)
                .collection("messages")
                .add(message)
                .addOnSuccessListener(documentReference -> {
                    messageInput.setText("");
                })
                .addOnFailureListener(e -> {
                    Toast.makeText(ChatActivity.this, "Error sending message", Toast.LENGTH_SHORT).show();
                });
    }

    private void listenForMessages() {
        db.collection("chats")
                .document(chatRoomId)
                .collection("messages")
                .orderBy("timestamp")
                .addSnapshotListener((value, error) -> {
                    if (error != null) {
                        return;
                    }

                    messageList.clear();
                    // Add welcome message back
                    addWelcomeMessage();

                    for (QueryDocumentSnapshot doc : value) {
                        ChatMessage message = doc.toObject(ChatMessage.class);
                        messageList.add(message);
                    }

                    chatAdapter.notifyDataSetChanged();
                    recyclerView.scrollToPosition(messageList.size() - 1);
                });
    }

}
