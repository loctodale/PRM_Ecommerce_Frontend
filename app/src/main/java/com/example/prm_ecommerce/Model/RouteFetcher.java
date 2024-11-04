package com.example.prm_ecommerce.Model;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;

import java.io.IOException;

public class RouteFetcher {

    public void fetchRoute(double startLat, double startLon, double endLat, double endLon) {
        OkHttpClient client = new OkHttpClient();

        String url = "http://router.project-osrm.org/route/v1/driving/" +
                startLon + "," + startLat + ";" + endLon + "," + endLat +
                "?overview=full&geometries=geojson";

        Request request = new Request.Builder().url(url).build();

        client.newCall(request).enqueue(new Callback() {
            @Override
            public void onFailure(Call call, IOException e) {
                e.printStackTrace();
            }

            @Override
            public void onResponse(Call call, Response response) throws IOException {
                if (response.isSuccessful()) {
                    String jsonResponse = response.body().string();
                    // Process the JSON response and display route on the map
                }
            }
        });
    }
}
