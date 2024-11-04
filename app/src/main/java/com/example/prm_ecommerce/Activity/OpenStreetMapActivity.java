package com.example.prm_ecommerce.Activity;

import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.util.Log;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import com.example.prm_ecommerce.R;

import org.json.JSONArray;
import org.json.JSONObject;
import org.osmdroid.config.Configuration;
import org.osmdroid.tileprovider.tilesource.TileSourceFactory;
import org.osmdroid.util.GeoPoint;
import org.osmdroid.views.MapView;
import org.osmdroid.views.overlay.Marker;
import org.osmdroid.views.overlay.Polyline;

import me.pushy.sdk.BuildConfig;
import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;

import java.io.IOException;

public class OpenStreetMapActivity extends AppCompatActivity {
    private MapView map;
    private OkHttpClient client;
    private double LongLoc;
    private double LatLoc;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Configuration.getInstance().setUserAgentValue(getPackageName());
        Configuration.getInstance().load(getApplicationContext(), PreferenceManager.getDefaultSharedPreferences(this));
        Configuration.getInstance().setUserAgentValue(BuildConfig.APPLICATION_ID);
        setContentView(R.layout.activity_open_street_map);
        LongLoc = Double.parseDouble((String) getIntent().getSerializableExtra("longLoc"));
        LatLoc = Double.parseDouble((String) getIntent().getSerializableExtra("latLoc"));
        map = findViewById(R.id.mapView);
        map.setTileSource(TileSourceFactory.MAPNIK);
        map.setTileSource(TileSourceFactory.DEFAULT_TILE_SOURCE);
        map.getTileProvider().clearTileCache();
        map.setMultiTouchControls(true);
        GeoPoint currentPosition = new GeoPoint(10.8751365, 106.7981484);
        map.getController().setCenter(currentPosition);
        map.getController().setZoom(15); // Set an appropriate zoom level

        // Initialize OkHttpClient
        client = new OkHttpClient();

        // Call the method to fetch route and display it
        fetchRouteAndDisplay(10.8751365, 106.7981484 , LongLoc, LatLoc); // Sample coordinates (San Francisco to Los Angeles)
    }


    private void fetchRouteAndDisplay(double startLon, double startLat, double endLon, double endLat) {
        // Construct the URL for OSRM API
        String url = "http://router.project-osrm.org/route/v1/driving/" +
                startLat + "," + startLon + ";" + endLat + "," + endLon +
                "?overview=full&geometries=geojson";

        Request request = new Request.Builder().url(url).build();

        // Execute the API call asynchronously
        client.newCall(request).enqueue(new Callback() {
            @Override
            public void onFailure(Call call, IOException e) {
                Log.d("Open street map", e.getMessage());
            }

            @Override
            public void onResponse(Call call, Response response) throws IOException {
                if (response.isSuccessful()) {
                    String jsonResponse = response.body().string();

                    displayRouteOnMap(jsonResponse);
                    // Call displayRouteOnMap() on the main thread
                    runOnUiThread(() -> {
                        displayRouteOnMap(jsonResponse);
                        addMarkers(startLon,startLat , endLon, endLat);
                    });
                }
            }
        });
    }

    /**
     * Parses JSON response and displays route on the map.
     *
     * @param jsonResponse JSON string from OSRM API containing route information.
     */
    private void displayRouteOnMap(String jsonResponse) {
        try {
            JSONObject jsonObject = new JSONObject(jsonResponse);
            JSONArray coordinates = jsonObject
                    .getJSONArray("routes")
                    .getJSONObject(0)
                    .getJSONObject("geometry")
                    .getJSONArray("coordinates");

            Polyline polyline = new Polyline();
            for (int i = 0; i < coordinates.length(); i++) {
                JSONArray coord = coordinates.getJSONArray(i);
                double lon = coord.getDouble(0);
                double lat = coord.getDouble(1);
                polyline.addPoint(new GeoPoint(lat, lon));
            }

            map.getOverlayManager().add(polyline);
            map.invalidate(); // Refresh the map to display the polyline
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void addMarkers(double startLat, double startLon, double endLat, double endLon) {
        // Create a marker for the starting point
        Marker startMarker = new Marker(map);
        startMarker.setPosition(new GeoPoint(startLat, startLon));
//        startMarker.setAnchor(Marker.ANCHOR_CENTER, Marker.ANCHOR_BOTTOM);
        startMarker.setIcon(getResources().getDrawable(R.drawable.ic_send));
        startMarker.setTitle("Start Point");
        map.getOverlays().add(startMarker);

        // Create a marker for the ending point
        Marker endMarker = new Marker(map);
        endMarker.setPosition(new GeoPoint(endLat, endLon));
        endMarker.setAnchor(Marker.ANCHOR_CENTER, Marker.ANCHOR_BOTTOM);
        endMarker.setTitle("End Point");
        map.getOverlays().add(endMarker);

        // Optionally, center the map on the starting point with a suitable zoom level
        map.getController().setZoom(14.0);
        map.getController().setCenter(new GeoPoint(startLat, startLon));

        map.invalidate(); // Refresh the map to show the markers
    }
}
