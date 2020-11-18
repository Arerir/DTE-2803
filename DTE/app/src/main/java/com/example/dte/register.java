package com.example.dte;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.os.Looper;
import android.view.View;
import android.widget.EditText;

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

public class register extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);
    }

    public void onClickBtn(View v)
    {

        EditText name = (EditText) findViewById(R.id.name);
        EditText password = (EditText) findViewById(R.id.password);
        EditText firstName = (EditText) findViewById(R.id.fødselsnummer);
        EditText email = (EditText) findViewById(R.id.epostadressen);
        Map<String, String> reg = new HashMap<String, String>();
        reg.put("email", email.getText().toString());
        reg.put("password", password.getText().toString());
        reg.put("firstName", firstName.getText().toString());
        reg.put("birthId", name.getText().toString());

        String s = makeRequest("https://danieleli2.asuscomm.com/api/users", reg);
        Intent intent = new Intent(register.this, MainActivity.class);
        startActivity(intent);
    }

    public static String makeRequest(final String uri, final Map<String, String> obj) {
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

    public static String makeRequest(final String uri) {
        final String[] value = new String[1];
        Thread t = new Thread() {

            public void run() {
                Looper.prepare(); //For Preparing Message Pool for the child Thread
                HttpClient client = new DefaultHttpClient();
                HttpConnectionParams.setConnectionTimeout(client.getParams(), 10000); //Timeout Limit
                HttpResponse response;

                try {
                    HttpGet get = new HttpGet(uri);
                    response = client.execute(get);
                    HttpEntity entity = response.getEntity();
                    String responseString = EntityUtils.toString(entity, "UTF-8");
                    value[0] = responseString;
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
}