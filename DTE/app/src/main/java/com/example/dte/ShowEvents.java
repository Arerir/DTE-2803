package com.example.dte;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.example.dte.APICalls.Call;
import com.example.dte.APICalls.Helper;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONArray;

import java.lang.reflect.Type;
import java.util.List;


public class ShowEvents extends AppCompatActivity {
    private static final String TAG = "ShowEvents";
    private Call call;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_show_events);

        // Init recyclerview and adapter
        RecyclerView recyclerView = (RecyclerView) findViewById(R.id.my_recycler_view);
        recyclerView.setLayoutManager(new LinearLayoutManager(getApplicationContext()));

        Helper.nuke();
        call = Call.getInstance(getCacheDir());
        call.getBadEvents(new Response.Listener<JSONArray>() {
            @Override
            public void onResponse(JSONArray response) {
                Log.d(TAG, "SUCCESS: " + response.toString());
                Gson gson = new Gson();
                Type listType = new TypeToken<List<BadEvent>>() {}.getType();
                List<BadEvent> events = gson.fromJson(response.toString(), listType);
                DataAdapter adapter = new DataAdapter(getApplicationContext(), events);
                recyclerView.setAdapter(adapter);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Log.d(TAG, "ERROR: " + error.getMessage());
            }
        });
    }
    public void onClickBtn1(View v)
    {
        Intent intent = new Intent(ShowEvents.this, MainActivity.class);
        startActivity(intent);
    }
    public void onClickBtn2(View v)
    {
        Intent intent = new Intent(ShowEvents.this, NavigationEvents.class);
        startActivity(intent);
    }
}