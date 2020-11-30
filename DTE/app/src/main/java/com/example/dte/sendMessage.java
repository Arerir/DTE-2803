package com.example.dte;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.os.Looper;
import android.view.View;
import android.widget.EditText;

import com.example.dte.APICalls.Helper;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.security.cert.CertificateException;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;

import javax.net.ssl.HostnameVerifier;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSession;
import javax.net.ssl.SSLSocketFactory;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;

import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

public class sendMessage extends AppCompatActivity {
    private static final DateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_send_message);
    }

    public void onClickBtn(View v)
    {

        EditText message = (EditText) findViewById(R.id.message);
        Bundle arguments = getIntent().getExtras();
        Map<String, Object> reg = new HashMap<String, Object>();
        reg.put("eventId", Integer.parseInt(arguments.get("id").toString()));
        reg.put("message", message.getText().toString());
        reg.put("eventDate", arguments.get("date"));
        Date date = new Date();
        reg.put("reminderDate", sdf.format(date));
        System.out.println(reg);
        makeRequest("https://danieleli2.asuscomm.com/api/Reminders/", reg);
        Intent intent = new Intent(sendMessage.this, Events.class);
        intent.putExtra("id", Integer.parseInt(arguments.get("id").toString()));
        startActivity(intent);
    }

    public static void makeRequest(final String uri, final Map<String, Object> obj) {
        final String[] value = new String[1];
        Thread t = new Thread() {

            public void run() {
                Looper.prepare(); //For Preparing Message Pool for the child Thread
                OkHttpClient client = Helper.getUnsafeOkHttpClient().build();

                MediaType mediaType = MediaType.parse("application/json");



                JSONObject json = new JSONObject();

                for (String i : obj.keySet()) {
                    try {
                        json.put(i, obj.get(i));
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }
                }
                RequestBody requestBody = null;
                Request request = null;
                try {
                    requestBody = RequestBody.create(mediaType, String.valueOf(json));
                    request = new Request.Builder()
                            .url(uri)
                            .post(requestBody)
                            .addHeader("Content-Type", "application/json")
                            .build();

                } catch (Exception e ) { }
                Response response = null;
                try {
                    response = client.newCall(request).execute();
                    System.out.println(response);
                } catch (IOException e) {
                    e.printStackTrace();
                }
                try {

                    value[0] = response.body().string();
                } catch (IOException e) {
                    e.printStackTrace();
                }
//                } catch(Exception e) {
//                    e.printStackTrace();
//                    System.out.println(e);
//
//                }

                Looper.loop(); //Loop in the message queue
            }
        };

        t.start();
        try {
            Thread.sleep(1500);
        } catch(InterruptedException e) {
            System.out.println("got interrupted!");
        }
//        return value[0];
    }



}