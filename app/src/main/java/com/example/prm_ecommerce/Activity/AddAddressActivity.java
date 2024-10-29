package com.example.prm_ecommerce.Activity;

import android.content.Intent;
import android.location.Address;
import android.location.Geocoder;
import android.location.Location;
import android.os.Bundle;
import android.os.PersistableBundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.MapView;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.Marker;
import com.google.android.gms.maps.model.MarkerOptions;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.example.prm_ecommerce.R;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class AddAddressActivity extends AppCompatActivity implements OnMapReadyCallback {
    private GoogleMap mMap;
    private MapView mapView;
    private static final String MAP_VIEW_BUNDLE_KEY = "MapViewBundleKey";
    private static final float DEFAULT_ZOOM = 15f;
    EditText etSearchAddress;
    TextView tvDistance;
    TextView tvAddress;
    TextView tvShipFee;
    Button btnSearch;
    Button btnOk;
    FusedLocationProviderClient fusedLocationProviderClient;

    double end_latitude = 0.0;
    double end_longtitude = 0.0;
    MarkerOptions origin ;
    MarkerOptions destination;
    double latitude = 10.8753014;
    double longtitude = 106.7999996;

    private Marker lastMarker; // Biến để lưu trữ marker cuối cùng
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_add_address);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        mapView = findViewById(R.id.mvMapView);

        etSearchAddress = findViewById(R.id.etAddress);
        btnSearch = findViewById(R.id.btnSearch);
        btnOk = findViewById(R.id.btnOk);
        tvDistance = findViewById(R.id.tvDistance);
        tvAddress = findViewById(R.id.tvAddress);
        tvShipFee = findViewById(R.id.tvShipFee);
        Bundle mapViewBundle = new Bundle();

        if (savedInstanceState!= null){
            mapViewBundle = savedInstanceState.getBundle(MAP_VIEW_BUNDLE_KEY);
        }

        mapView.onCreate(mapViewBundle);
        mapView.getMapAsync(this);

        btnSearch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                searchArea();
            }
        });

        btnOk.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                clickBtnOk();
            }
        });
    }

    private void clickBtnOk() {
        // Lấy địa chỉ và phí ship
        String address = tvAddress.getText().toString();
        String shippingFee = tvShipFee.getText().toString();

        // Tạo Intent để quay về trang giỏ hàng
        Intent intent = new Intent(AddAddressActivity.this, CartActivity.class);
        intent.putExtra("ADDRESS", address); // Truyền địa chỉ
        intent.putExtra("SHIPPING_FEE", shippingFee); // Truyền phí ship

        startActivity(intent); // Khởi chạy CartActivity
        finish(); // Kết thúc AddAddressActivity nếu không cần quay lại
    }

    private void searchArea() {
        String location = etSearchAddress.getText().toString();
        List<Address> addressList = new ArrayList<>();
        MarkerOptions markerOptions = new MarkerOptions();

        if (!location.isEmpty()) {
            Geocoder geocoder = new Geocoder(getApplicationContext());
            try {
                addressList = geocoder.getFromLocationName(location, 5);
            } catch (IOException e) {
                e.printStackTrace();
            }

            if (addressList != null && !addressList.isEmpty()) {
                // Lấy địa chỉ đầu tiên từ danh sách kết quả
                Address myAddress = addressList.get(0);
                LatLng latLng = new LatLng(myAddress.getLatitude(), myAddress.getLongitude());

                // Nếu marker trước đó đã tồn tại, xóa nó
                if (lastMarker != null) {
                    lastMarker.remove();
                }

                // Tạo marker mới và lưu vào biến lastMarker
                markerOptions.position(latLng);
                lastMarker = mMap.addMarker(markerOptions);
                mMap.animateCamera(CameraUpdateFactory.newLatLng(latLng));

                // Lưu tọa độ đích
                end_latitude = myAddress.getLatitude();
                end_longtitude = myAddress.getLongitude();

                // Tính khoảng cách giữa vị trí mặc định và vị trí tìm kiếm
                float[] results = new float[1];
                Location.distanceBetween(latitude, longtitude, end_latitude, end_longtitude, results);
                double distanceInKm = results[0] / 1000;  // Chuyển sang km

                // Tính phí vận chuyển
                int shippingFee = (int) ((int)distanceInKm * 5000); // Tính phí ship

                // Hiển thị địa chỉ và khoảng cách trong các TextView
                tvAddress.setText(myAddress.getAddressLine(0));
                tvDistance.setText(String.format("%.1f", distanceInKm));
                tvShipFee.setText(String.valueOf(shippingFee)); // Hiển thị phí vận chuyển

            }
        } else {
            Toast.makeText(this, "Vui lòng nhập địa chỉ", Toast.LENGTH_SHORT).show();
        }
    }
    @Override
    public void onSaveInstanceState(@NonNull Bundle outState, @NonNull PersistableBundle outPersistentState) {
        super.onSaveInstanceState(outState, outPersistentState);

        Bundle mapViewBundle = outState.getBundle(MAP_VIEW_BUNDLE_KEY);
        if (mapViewBundle == null){
            mapViewBundle = new  Bundle();
            outState.putBundle(MAP_VIEW_BUNDLE_KEY, mapViewBundle);
        }

        mapView.onSaveInstanceState(mapViewBundle);
    }


    private void moveCamera(LatLng latLng, Float zoom){
        mMap.moveCamera(CameraUpdateFactory.newLatLngZoom(latLng, zoom));
    }

    @Override
    public void onMapReady(@NonNull GoogleMap googleMap) {
        mapView.onResume();
        mMap = googleMap;

        // Di chuyển camera đến vị trí mặc định
        LatLng defaultLocation = new LatLng(latitude, longtitude);
        moveCamera(defaultLocation, DEFAULT_ZOOM);

        // Đánh dấu vị trí mặc định trên bản đồ
        mMap.addMarker(new MarkerOptions().position(defaultLocation).title("Vị trí sẵn có"));
    }

    @Override
    public void onPointerCaptureChanged(boolean hasCapture) {
        super.onPointerCaptureChanged(hasCapture);
    }
}