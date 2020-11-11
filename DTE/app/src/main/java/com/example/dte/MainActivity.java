package com.example.dte;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Looper;
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

import com.google.gson.Gson;
import com.google.gson.JsonObject;



import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicHeader;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.protocol.HTTP;
import org.apache.http.util.EntityUtils;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.StringWriter;
import java.security.cert.CertificateException;
import java.util.HashMap;
import java.util.Map;
import java.util.Properties;

import javax.net.ssl.HostnameVerifier;
<<<<<<< HEAD
=======
import javax.net.ssl.HttpsURLConnection;
>>>>>>> da8d0df098ad30c5bd6957a644c6462dd315ca8a
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSession;
import javax.net.ssl.SSLSocketFactory;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;

<<<<<<< HEAD
import okhttp3.FormBody;
import okhttp3.MediaType;
=======
>>>>>>> da8d0df098ad30c5bd6957a644c6462dd315ca8a
import okhttp3.MultipartBody;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;
import okio.Buffer;


public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        final Properties props = System.getProperties();

        props.setProperty("jdk.internal.httpclient.disableHostnameVerification", Boolean.TRUE.toString());
    }
    public void onClickBtn(View v)
    {

        EditText txt1 = (EditText) findViewById(R.id.name);
        EditText txt2 = (EditText) findViewById(R.id.password);
        Map<String, String> login = new HashMap<String, String>();
        login.put("userId", txt1.getText().toString());
        login.put("password", txt2.getText().toString());
        Boolean s = makeRequest("https://danieleli2.asuscomm.com/api/users/authenticate", login);
//        JsonObject convertedObject = new Gson().fromJson(s, JsonObject.class);
//
//        System.out.println(convertedObject.toString());
        if(s == true) {
            Intent intent = new Intent(MainActivity.this, NavigationEvents.class);
            startActivity(intent);
        }
    }

    public static Boolean makeRequest(final String uri, final Map<String, String> obj) {
        final String[] value = new String[1];
        Thread t = new Thread() {

            public void run() {
                Looper.prepare(); //For Preparing Message Pool for the child Thread
                OkHttpClient client = getUnsafeOkHttpClient();

                MediaType mediaType = MediaType.parse("application/json");

<<<<<<< HEAD

=======
                
                RequestBody requestBody = new MultipartBody.Builder()
                        .setType(MultipartBody.FORM)
                        .addFormDataPart("userId", obj.get("userId"))
                        .addFormDataPart("password", obj.get("password"))
                        .build();
>>>>>>> da8d0df098ad30c5bd6957a644c6462dd315ca8a

                JSONObject json = new JSONObject();

                for (String i : obj.keySet()) {
                    try {
                        json.put(i, obj.get(i));
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }
                }
                RequestBody requestBody = RequestBody.create(mediaType, String.valueOf(json));
                Request request = new Request.Builder()
                        .url(uri)
                        .post(requestBody)
                        .addHeader("Content-Type", "application/json")
                        .build();


//                DefaultHttpClient client = new DefaultHttpClient();
//                HttpConnectionParams.setConnectionTimeout(client.getParams(), 10000); //Timeout Limit
//                HttpResponse response;
//                JSONObject json = new JSONObject();
//
//                try {
//                    HttpPost post = new HttpPost(uri);
//                    for (String i : obj.keySet()) {
//                        json.put(i, obj.get(i));
//                    }
//
//                    StringEntity se = new StringEntity(json.toString());
//                    se.setContentType(new BasicHeader(HTTP.CONTENT_TYPE, "application/json"));
//                    post.setEntity(se);
//                    response = client.execute(post);
//
//                    HttpEntity entity = response.getEntity();
//                    String responseString = EntityUtils.toString(entity, "UTF-8");
//                Buffer buffer = new Buffer();
//                try {
//                    request.body().writeTo(buffer);
//                } catch (IOException e) {
//                    e.printStackTrace();
//                }
                
                Response response = null;
                try {
                    response = client.newCall(request).execute();
                } catch (IOException e) {
                    e.printStackTrace();
                }
<<<<<<< HEAD
                try {

                    value[0] = response.body().string();
                } catch (IOException e) {
                    e.printStackTrace();
                }
=======
                if(response != null) System.out.println(response.body().toString());

>>>>>>> da8d0df098ad30c5bd6957a644c6462dd315ca8a
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
        return Boolean.parseBoolean(value[0]);
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

//    public static String makeRequest(final String uri) {
//        final String[] value = new String[1];
//        Thread t = new Thread() {
//
//            public void run() {
//                Looper.prepare(); //For Preparing Message Pool for the child Thread
//                HttpClient client = new DefaultHttpClient();
//                HttpConnectionParams.setConnectionTimeout(client.getParams(), 10000); //Timeout Limit
//                HttpResponse response;
//
//                try {
//                    HttpGet get = new HttpGet(uri);
//                    response = client.execute(get);
//                    HttpEntity entity = response.getEntity();
//                    String responseString = EntityUtils.toString(entity, "UTF-8");
//                    value[0] = responseString;
//                } catch(Exception e) {
//                    e.printStackTrace();
//                    System.out.println(e);
//
//                }
//
//                Looper.loop(); //Loop in the message queue
//            }
//        };
//
//        t.start();
//        try {
//            Thread.sleep(1500);
//        } catch(InterruptedException e) {
//            System.out.println("got interrupted!");
//        }
//        return value[0];
//    }

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