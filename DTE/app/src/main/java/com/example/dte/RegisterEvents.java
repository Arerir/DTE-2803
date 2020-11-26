package com.example.dte;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.os.Looper;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.Spinner;

import com.example.dte.APICalls.Helper;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.security.cert.CertificateException;
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

public class RegisterEvents extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register_events);


        Spinner dropdown = findViewById(R.id.spinner1);
        String[] items = new String[]{"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"};
        ArrayAdapter<String> adapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_dropdown_item, items);
        dropdown.setAdapter(adapter);

        Spinner dropdown1 = findViewById(R.id.spinner2);
        String[] items1 = new String[]{"Akuttmottak", "Ambulanseavdelingen", "Anestesi og operasjon", "Barne og ungdomsklinikken", "Blodbanken",
                "Blodprøvetaking", "Diagnostisk klinikk", "Felles poliklinikk", "Fødeavdelingen", "Intensiv"};
        ArrayAdapter<String> adapter1 = new ArrayAdapter<>(this, android.R.layout.simple_spinner_dropdown_item, items1);
        dropdown1.setAdapter(adapter1);
    }

    public void onClickBtn1(View v) {
        EditText message = (EditText) findViewById(R.id.arsak);
        EditText reason = (EditText) findViewById(R.id.beskrivelse);
        Spinner placement = (Spinner) findViewById(R.id.spinner2);
        Spinner serverityId = (Spinner) findViewById(R.id.spinner1);
        DatePicker date = (DatePicker) findViewById(R.id.dp_datepicker);

        Map<String, Object> reg = new HashMap<String, Object>();
        reg.put("message", message.getText().toString());
        reg.put("reason", reason.getText().toString());
        reg.put("placement", placement.getSelectedItem().toString());
        reg.put("severityId", Integer.parseInt(serverityId.getSelectedItem().toString()));
        reg.put("date", date.getYear()+"-"+(date.getMonth()+1)+"-"+date.getDayOfMonth());

        String s = makeRequest("https://danieleli2.asuscomm.com/api/BadEvents/", reg);
        Intent intent = new Intent(RegisterEvents.this, NavigationEvents.class);
        startActivity(intent);
    }

    public static String makeRequest(final String uri, final Map<String, Object> obj) {
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
                System.out.println(json);
                RequestBody requestBody = null;
                Request request = null;
                try {
                    requestBody = RequestBody.create(mediaType, String.valueOf(json));
                    request = new Request.Builder()
                            .url(uri)
                            .post(requestBody)
                            .addHeader("Content-Type", "application/json")
                            .build();

                } catch (Exception e) {
                    System.out.println(e);
                }


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
        } catch (InterruptedException e) {
            System.out.println("got interrupted!");
        }

        return value[0];
    }
}