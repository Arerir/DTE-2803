package com.example.dte.APICalls;

import org.json.JSONArray;
import org.json.JSONObject;

import com.android.volley.Cache;
import com.android.volley.Network;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.BasicNetwork;
import com.android.volley.toolbox.DiskBasedCache;
import com.android.volley.toolbox.HurlStack;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.JsonObjectRequest;
import com.example.dte.BadEvent;

import java.io.File;
import java.util.List;


public class Call {
    private static Call single_instance;
    private static final String baseUrl = "https://danieleli2.asuscomm.com/api/";
    private Cache cache;
    private Network network;
    private RequestQueue requestQueue;

    private Call(File cacheDir) {
        cache =  new DiskBasedCache(cacheDir, 1024 * 1024); // 1MB cap
        network =  new BasicNetwork(new HurlStack());
        requestQueue = new RequestQueue(cache, network);
        requestQueue.start();
    }

    public static Call getInstance(File cacheDir) {
        if (single_instance == null) {
            single_instance = new Call(cacheDir);
        }
        return single_instance;
    }

    public void getBadEvents(Response.Listener<JSONArray> listener, Response.ErrorListener errorListener) {
        String url = baseUrl + "BadEvents";
        JsonArrayRequest request = new JsonArrayRequest(url, listener, errorListener);
        requestQueue.add(request);
    }
}
