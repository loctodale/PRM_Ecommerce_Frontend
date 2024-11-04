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
import com.google.android.gms.maps.model.Polyline;
import com.google.android.gms.maps.model.PolylineOptions;
import com.example.prm_ecommerce.R;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class AddAddressActivity extends AppCompatActivity implements OnMapReadyCallback {
    private GoogleMap mMap;
    private MapView mapView;
    private static final String MAP_VIEW_BUNDLE_KEY = "MapViewBundleKey";
    private static final float DEFAULT_ZOOM = 15f;
    private EditText etSearchAddress;
    private TextView tvDistance;
    private TextView tvAddress;
    private TextView tvShipFee;
    private Button btnSearch;
    private Button btnOk;
    private FusedLocationProviderClient fusedLocationProviderClient;

    private double end_latitude = 0.0;
    private double end_longtitude = 0.0;
    private double latitude = 10.8753014;
    private double longtitude = 106.7999996;

    private Marker lastMarker;  // Marker của vị trí cuối cùng được tìm kiếm
    private Polyline polyline;  // Đường nối giữa hai địa điểm

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

        Bundle mapViewBundle = savedInstanceState != null ? savedInstanceState.getBundle(MAP_VIEW_BUNDLE_KEY) : new Bundle();
        mapView.onCreate(mapViewBundle);
        mapView.getMapAsync(this);

        btnSearch.setOnClickListener(view -> searchArea());
        btnOk.setOnClickListener(view -> clickBtnOk());
    }

    private void clickBtnOk() {
        // Lấy địa chỉ và phí ship
        String address = tvAddress.getText().toString();
        String shippingFee = tvShipFee.getText().toString();

        // Tạo Intent để quay về trang giỏ hàng
        Intent intent = new Intent(AddAddressActivity.this, CartActivity.class);
        intent.putExtra("ADDRESS", address); // Truyền địa chỉ
        intent.putExtra("DELIVERY_FEE", shippingFee); // Truyền phí ship
        intent.putExtra("LONG", end_longtitude);
        intent.putExtra("LAT", end_latitude);
        startActivity(intent); // Khởi chạy CartActivity
        finish(); // Kết thúc AddAddressActivity nếu không cần quay lại
    }

    private void searchArea() {
        String location = etSearchAddress.getText().toString();
        List<Address> addressList = new ArrayList<>();

        if (!location.isEmpty()) {
            Geocoder geocoder = new Geocoder(getApplicationContext());
            try {
                addressList = geocoder.getFromLocationName(location, 5);
            } catch (IOException e) {
                e.printStackTrace();
            }

            if (addressList != null && !addressList.isEmpty()) {
                Address myAddress = addressList.get(0);
                LatLng latLng = new LatLng(myAddress.getLatitude(), myAddress.getLongitude());

                // Nếu marker và polyline trước đó đã tồn tại, xóa chúng
                if (lastMarker != null) {
                    lastMarker.remove();
                }
                if (polyline != null) {
                    polyline.remove();
                }

                // Tạo marker mới và vẽ trên bản đồ
                MarkerOptions markerOptions = new MarkerOptions().position(latLng).title("Vị trí tìm kiếm");
                lastMarker = mMap.addMarker(markerOptions);
                mMap.animateCamera(CameraUpdateFactory.newLatLng(latLng));

                // Cập nhật tọa độ đích
                end_latitude = myAddress.getLatitude();
                end_longtitude = myAddress.getLongitude();

                // Tính khoảng cách và phí ship
                float[] results = new float[1];
                Location.distanceBetween(latitude, longtitude, end_latitude, end_longtitude, results);
                double distanceInKm = results[0] / 1000;  // Chuyển sang km
                int shippingFee = (int)distanceInKm * 5000; // Tính phí ship

                // Cập nhật thông tin lên TextView
                tvAddress.setText(myAddress.getAddressLine(0));
                tvDistance.setText(String.format("%.1f km", distanceInKm));
                tvShipFee.setText(String.valueOf(shippingFee)); // Hiển thị phí vận chuyển

                // Vẽ đường nối giữa vị trí mặc định và vị trí tìm kiếm
                polyline = mMap.addPolyline(new PolylineOptions()
                        .add(new LatLng(latitude, longtitude), latLng)
                        .color(getResources().getColor(R.color.purple_Light))
                        .width(5f));
            }
        } else {
            Toast.makeText(this, "Vui lòng nhập địa chỉ", Toast.LENGTH_SHORT).show();
        }
    }

    @Override
    public void onSaveInstanceState(@NonNull Bundle outState, @NonNull PersistableBundle outPersistentState) {
        super.onSaveInstanceState(outState, outPersistentState);
        Bundle mapViewBundle = outState.getBundle(MAP_VIEW_BUNDLE_KEY);
        if (mapViewBundle == null) {
            mapViewBundle = new Bundle();
            outState.putBundle(MAP_VIEW_BUNDLE_KEY, mapViewBundle);
        }
        mapView.onSaveInstanceState(mapViewBundle);
    }

    private void moveCamera(LatLng latLng, Float zoom) {
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