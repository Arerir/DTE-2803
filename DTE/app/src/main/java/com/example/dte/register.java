package com.example.dte;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.os.Looper;
import android.view.View;
import android.widget.EditText;

import com.example.dte.APICalls.Helper;

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

        makeRequest("https://danieleli2.asuscomm.com/api/Users", reg);
        Intent intent = new Intent(register.this, MainActivity.class);
        startActivity(intent);
    }

    public static void makeRequest(final String uri, final Map<String, String> obj) {
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
        try {
            t.join();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
//        return value[0];
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