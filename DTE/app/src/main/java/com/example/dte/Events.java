package com.example.dte;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.os.Looper;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.security.cert.CertificateException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
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

public class Events extends AppCompatActivity {

    BadEvent data = null;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_events);
        Bundle arguments = getIntent().getExtras();

        String s = makeRequest("https://danieleli2.asuscomm.com/api/BadEvents/"+ arguments.get("id"));

        Gson g = new Gson();

        data = g.fromJson(s, BadEvent.class);

        TextView txt1 = (TextView) findViewById(R.id.textView20);
        TextView txt2 = (TextView) findViewById(R.id.textView23);
        TextView txt3 = (TextView) findViewById(R.id.textView25);
        TextView txt4 = (TextView) findViewById(R.id.textView26);
        TextView txt5 = (TextView) findViewById(R.id.textView27);
        TextView txt6 = (TextView) findViewById(R.id.textView30);
        Button btn1 = (Button) findViewById(R.id.button7);

        txt1.setText(data.getPlacement());
        txt2.setText(data.getMessage());
        txt3.setText(String.valueOf(data.getSeverityId()));
        txt4.setText(data.getReason());
        txt5.setText(data.getStatusId() == 1 ? "Under behandling" : "Ferdig behandlet");
        txt6.setVisibility(data.getStatusId() == 1 ? View.INVISIBLE : View.VISIBLE);
        btn1.setVisibility(data.getStatusId() == 1 ? View.INVISIBLE : View.VISIBLE);
    }


    public void onClickArchive(View v){
        Map<String, Object> reg = new HashMap<String, Object>();
        reg.put("message", data.getMessage());
        reg.put("reason", data.getReason());
        reg.put("placement", data.getPlacement());
        reg.put("severityId", data.getSeverityId());
        reg.put("statusId", data.getStatusId());
        reg.put("id", data.getId());
        reg.put("date", data.getDate());

        makeRequest("https://danieleli2.asuscomm.com/api/BadEvents/archive/"+ data.getId(), reg);
        Intent intent = new Intent(Events.this, NavigationEvents.class);
        startActivity(intent);
    }
    public void onSendMessage(View v){
        Intent intent = new Intent(Events.this, sendMessage.class);
        intent.putExtra("id", data.getId());
        intent.putExtra("date", data.getDate());
        startActivity(intent);
    }

    public void onClickShowMessage(View v){
        String s = makeRequest("https://danieleli2.asuscomm.com/api/Reminders/");
        Gson g = new Gson();
        JSONArray jsonArr = null;
        try {
            jsonArr = new JSONArray(s);
        } catch (JSONException e) {
            e.printStackTrace();
        }

        for (int i = 0; i < jsonArr.length(); i++)
        {
            try {
                Toast.makeText(this, jsonArr.getJSONObject(i).get("message").toString(), Toast.LENGTH_LONG).show();
                makeRequestDelete("https://danieleli2.asuscomm.com/api/Reminders/" + jsonArr.getJSONObject(i).get("id"));
//               System.out.println(jsonArr.getJSONObject(i).get("id"));
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }

    }

    public static void makeRequest(final String uri, final Map<String, Object> obj) {
        final String[] value = new String[1];
        Thread t = new Thread() {

            public void run() {
                Looper.prepare(); //For Preparing Message Pool for the child Thread
                OkHttpClient client = getUnsafeOkHttpClient();

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
                            .put(requestBody)
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

    public static String makeRequestDelete(final String uri) {
        final String[] value = new String[1];
        Thread t = new Thread() {

            public void run() {
                Looper.prepare(); //For Preparing Message Pool for the child Thread
                OkHttpClient client = getUnsafeOkHttpClient();
                Response response;

                try {
                    Request request = new Request.Builder()
                            .url(uri)
                            .delete()
                            .build();
                    response = client.newCall(request).execute();
                    value[0] = response.body().string();
                } catch(Exception e) {
                    e.printStackTrace();
                    System.out.println(e);

                }

                Looper.loop(); //Loop in the message queue
            }
        };

        t.start();
        try {
            Thread.sleep(1500);
        } catch(InterruptedException e) {
            System.out.println("got interrupted!");
        }

        return value[0];
    }

    public static String makeRequest(final String uri) {
        final String[] value = new String[1];
        Thread t = new Thread() {

            public void run() {
                Looper.prepare(); //For Preparing Message Pool for the child Thread
                OkHttpClient client = getUnsafeOkHttpClient();
                Response response;

                try {
                    Request request = new Request.Builder()
                            .url(uri)
                            .get()
                            .build();
                    response = client.newCall(request).execute();
                    value[0] = response.body().string();
                } catch(Exception e) {
                    e.printStackTrace();
                    System.out.println(e);

                }

                Looper.loop(); //Loop in the message queue
            }
        };

        t.start();
        try {
            Thread.sleep(1500);
        } catch(InterruptedException e) {
            System.out.println("got interrupted!");
        }

        return value[0];
    }


    private static OkHttpClient getUnsafeOkHttpClient() {
        try {
            // Create a trust manager that does not validate certificate chains
            final TrustManager[] trustAllCerts = new TrustManager[] {
                    new X509TrustManager() {
                        @Override
                        public void checkClientTrusted(java.security.cert.X509Certificate[] chain, String authType) throws CertificateException {
                        }

                        @Override
                        public void checkServerTrusted(java.security.cert.X509Certificate[] chain, String authType) throws CertificateException {
                        }

                        @Override
                        public java.security.cert.X509Certificate[] getAcceptedIssuers() {
                            return new java.security.cert.X509Certificate[]{};
                        }
                    }
            };

            // Install the all-trusting trust manager
            final SSLContext sslContext = SSLContext.getInstance("SSL");
            sslContext.init(null, trustAllCerts, new java.security.SecureRandom());
            // Create an ssl socket factory with our all-trusting manager
            final SSLSocketFactory sslSocketFactory = sslContext.getSocketFactory();

            OkHttpClient.Builder builder = new OkHttpClient.Builder();
            builder.sslSocketFactory(sslSocketFactory, (X509TrustManager)trustAllCerts[0]);
            builder.hostnameVerifier(new HostnameVerifier() {
                @Override
                public boolean verify(String hostname, SSLSession session) {
                    return true;
                }
            });

            OkHttpClient okHttpClient = builder.build();
            return okHttpClient;
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }
}