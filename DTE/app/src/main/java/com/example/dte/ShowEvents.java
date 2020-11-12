package com.example.dte;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import java.util.ArrayList;
import java.util.List;

public class ShowEvents extends AppCompatActivity {

    List<Events> phones = new ArrayList<>();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_show_events);

        setInitialData();
        RecyclerView recyclerView = (RecyclerView) findViewById(R.id.my_recycler_view);
        // создаем адаптер
        DataAdapter adapter = new DataAdapter(this, phones);
        // устанавливаем для списка адаптер
        recyclerView.setAdapter(adapter);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));
    }

    private void setInitialData(){

        phones.add(new Events("Stor hendelse", "01.10.2020", "10"));
        phones.add(new Events("Liten hendelse", "02.10.2020","2"));
        phones.add(new Events("Middle hendelse", "05.10.2020", "5"));
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