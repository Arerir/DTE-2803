package com.example.dte;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import com.google.gson.GsonBuilder;

import java.util.HashMap;
import java.util.Map;

public class NavigationEvents extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_navigation_events);
    }
    public void onClickBtn1(View v)
    {
        Intent intent = new Intent(NavigationEvents.this, RegisterEvents.class);
        startActivity(intent);
    }
    public void onClickBtn2(View v)
    {
        Intent intent = new Intent(NavigationEvents.this, ShowEvents.class);
        startActivity(intent);
    }
    public void onClickBtn3(View v)
    {
        Intent intent = new Intent(NavigationEvents.this, MainActivity.class);
        startActivity(intent);
    }
}