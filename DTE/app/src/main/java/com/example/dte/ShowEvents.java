package com.example.dte;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

<<<<<<< HEAD
import android.content.Intent;
import android.os.Bundle;
import android.os.Looper;
import android.view.View;

import com.google.gson.Gson;
import com.google.gson.JsonArray;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.security.cert.CertificateException;
import java.util.ArrayList;
=======
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.example.dte.APICalls.Call;
import com.example.dte.APICalls.Helper;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONArray;

import java.lang.reflect.Type;
>>>>>>> 2f8af475c8f1fae6b10ac51856a78cec073cfb9e
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


public class ShowEvents extends AppCompatActivity {
    private static final String TAG = "ShowEvents";
    private Call call;

<<<<<<< HEAD
    List<BadEvent> events = new ArrayList<>();
=======
>>>>>>> 2f8af475c8f1fae6b10ac51856a78cec073cfb9e
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_show_events);

<<<<<<< HEAD
        events = setInitialData();
        RecyclerView recyclerView = (RecyclerView) findViewById(R.id.my_recycler_view);
        DataAdapter adapter = new DataAdapter(this, events);
        adapter.setOnItemClickListener(new DataAdapter.ItemClickListener(){
            @Override
            public void onItemClickListener(int pos, View v) {
                Intent intent = new Intent(ShowEvents.this, Events.class);
                intent.putExtra("id", events.get(pos).getId());
                startActivity(intent);
            }
        });

//        // устанавливаем для списка адаптер
        recyclerView.setAdapter(adapter);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));
    }

    private List<BadEvent> setInitialData(){
        List<BadEvent> b = new ArrayList<>();
        String s = makeRequest("https://danieleli2.asuscomm.com/api/BadEvents/");
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
                b.add(g.fromJson(jsonArr.getJSONObject(i).toString(), BadEvent.class));
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }
        return b;
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
=======
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
>>>>>>> 2f8af475c8f1fae6b10ac51856a78cec073cfb9e
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