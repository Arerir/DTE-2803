package com.example.dte;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.inputmethod.EditorInfo;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.GsonBuilder;

import org.apache.http.HttpResponse;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.util.HashMap;
import java.util.Map;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

    }
    public void onClickBtn(View v)
    {
        Map<String, String> comment = new HashMap<String, String>();
        String json = new GsonBuilder().create().toJson(comment, Map.class);
    }

    public static HttpResponse makeRequest(String uri, String json) { // funksjon for a jobbe med foresporsel til server
        try {
            HttpPost httpPost = new HttpPost(uri); // sett URL
            httpPost.setEntity(new StringEntity(json)); // add data
            httpPost.setHeader("Accept", "application/json");
            httpPost.setHeader("Content-type", "application/json");//angi sendetype data
            return new DefaultHttpClient().execute(httpPost); // utfore data til server
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        } catch (ClientProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return null;
    }
    public void onClickRegister(View v){
        Intent intent = new Intent(MainActivity.this, register.class);
        startActivity(intent);
    }
    public void onClickPassword(View v)
    {
        Toast.makeText(this, "Passordet er dit personlig \n" +
                "kode med fire tall", Toast.LENGTH_LONG).show();
    }
}