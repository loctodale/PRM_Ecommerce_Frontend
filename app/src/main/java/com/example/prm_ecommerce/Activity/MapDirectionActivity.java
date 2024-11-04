package com.example.prm_ecommerce.Activity;

import androidx.annotation.NonNull;
import androidx.core.app.ActivityCompat;
import androidx.fragment.app.FragmentActivity;

import android.Manifest;
import android.content.pm.PackageManager;
import android.location.Location;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;

import com.codebyashish.googledirectionapi.AbstractRouting;
import com.codebyashish.googledirectionapi.ErrorHandling;
import com.codebyashish.googledirectionapi.RouteDrawing;
import com.codebyashish.googledirectionapi.RouteInfoModel;
import com.codebyashish.googledirectionapi.RouteListener;
import com.example.prm_ecommerce.R;
import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.CameraPosition;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;
import com.example.prm_ecommerce.databinding.ActivityMapDirectionBinding;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.Task;

import java.util.ArrayList;

public class MapDirectionActivity extends FragmentActivity implements OnMapReadyCallback, RouteListener {

    private GoogleMap mMap;
    private ActivityMapDirectionBinding binding;
    FusedLocationProviderClient fusedLocationProviderClient;
    double userLat, userLong;
    private LatLng desnationLocation, userLocation;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        binding = ActivityMapDirectionBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(com.example.prm_ecommerce.R.id.map);
        mapFragment.getMapAsync(this);
        fusedLocationProviderClient = LocationServices.getFusedLocationProviderClient(this);
    }


    @Override
    public void onMapReady(GoogleMap googleMap) {
        mMap = googleMap;
        mMap.getUiSettings().setZoomControlsEnabled(true);

        userLat = 10.875536;
        userLong = 106.7997082;
        LatLng latlng= new LatLng(userLat, userLong);
        userLocation = latlng;
        mMap.addMarker(new MarkerOptions().position(latlng).title("Marker in HCM"));

        CameraPosition cameraPosition = new CameraPosition.Builder().target(latlng).zoom(12).build();
        mMap.animateCamera(CameraUpdateFactory.newCameraPosition(cameraPosition));

        mMap.setOnMapClickListener(new GoogleMap.OnMapClickListener() {
            @Override
            public void onMapClick(@NonNull LatLng latLng) {
                mMap.clear();
                desnationLocation = latLng;
                MarkerOptions markerOptions = new MarkerOptions();
                markerOptions.position(latLng);
                markerOptions.title("Select position");
                mMap.addMarker(markerOptions);

                getRoute(userLocation, desnationLocation);
            }
        });
    }

    private void getRoute(LatLng userLocation, LatLng desnationLocation) {
        RouteDrawing routeDrawing = new RouteDrawing.Builder()
                .context(MapDirectionActivity.this)  // pass your activity or fragment's context
                .travelMode(AbstractRouting.TravelMode.DRIVING)
                .withListener(this).alternativeRoutes(true)
                .waypoints(userLocation, desnationLocation)
                .build();
        routeDrawing.execute();
    }

    @Override
    public void onRouteFailure(ErrorHandling e) {
        Toast.makeText(this, e.getMessage(), Toast.LENGTH_SHORT).show();
        Log.d("Goole Api", e.getMessage());
    }

    @Override
    public void onRouteStart() {
        Toast.makeText(this, "Route Started", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onRouteSuccess(ArrayList<RouteInfoModel> list, int indexing) {
        Toast.makeText(this, "Route Success", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onRouteCancelled() {
        Toast.makeText(this, "Route Canceled", Toast.LENGTH_SHORT).show();
    }
}