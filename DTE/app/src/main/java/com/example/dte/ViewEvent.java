package com.example.dte;

import androidx.appcompat.app.AppCompatActivity;

import android.annotation.SuppressLint;
import android.os.Bundle;
import android.widget.TextView;
import android.widget.Toast;

import com.example.dte.APICalls.Call;

public class ViewEvent extends AppCompatActivity {
    private TextView registersName, description, reason, placement;
    private Call call;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_view_event);
        initViews();

        Bundle b = getIntent().getExtras();
        if(b != null) {
            BadEvent badEvent = b.getParcelable("badEvent");
            if (badEvent != null) {
                setViewData(badEvent);
            } else {
                displayError("badEvent is null");
            }
        } else {
            displayError("Could not get badEvent from recyclerview");
        }
    }

    private void displayError(String msg) {
        Toast.makeText(this, msg, Toast.LENGTH_SHORT).show();
    }

    private void initViews() {
        registersName = (TextView) findViewById(R.id.registersName);
        description = (TextView) findViewById(R.id.Description);
        reason = (TextView) findViewById(R.id.reason);
        placement = (TextView) findViewById(R.id.placement);
    }

    private void setViewData(BadEvent event) {
        registersName.setText("Missing"); // TODO missing createdById in BadEventDTO. therefore cannot get user responsible for the badEvent
        description.setText(event.getMessage());
        reason.setText(event.getReason());
        placement.setText(event.getPlacement());

        // TODO get severity, reminders and status from API.
        //call = Call.getInstance(getCacheDir());
    }
}