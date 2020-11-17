package com.example.dte;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.TextView;

import androidx.recyclerview.widget.RecyclerView;

import java.util.List;

class DataAdapter extends RecyclerView.Adapter<DataAdapter.ViewHolder> {

    private Context context;
    private List<BadEvent> events;

    DataAdapter(Context context, List<BadEvent> events) {
        this.events = events;
        this.context = context;
    }

    public void setEvents(List<BadEvent> events) {
        this.events = events;
        notifyDataSetChanged();
    }

    public void addItem(BadEvent event) {
        this.events.add(event);
        notifyItemInserted(this.events.size() - 1);
    }

    @Override
    public DataAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        return new DataAdapter.ViewHolder(LayoutInflater.from(parent.getContext()).inflate(R.layout.list_item, parent, false));
    }

    @Override
    public void onBindViewHolder(DataAdapter.ViewHolder holder, int position) {
        holder.onBind(position);
    }

    @Override
    public int getItemCount() {
        return events.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        TextView nameView, timeView, alvorlighetsgradView;
        LinearLayout root;
        ViewHolder(View view){
            super(view);
            root = (LinearLayout) view.findViewById(R.id.rootLinearLayout);
            nameView = (TextView) view.findViewById(R.id.name);
            timeView = (TextView) view.findViewById(R.id.time);
            alvorlighetsgradView = (TextView) view.findViewById(R.id.alvorlighetsgrad);
        }
        void onBind(int position) {
            final BadEvent badEvent = events.get(position);
            nameView.setText(String.valueOf(badEvent.getMessage()));
            timeView.setText(String.valueOf(badEvent.getDate()));
            alvorlighetsgradView.setText(String.valueOf(badEvent.getSeverityId()));

            // Onclick open ViewEvent to display selected badEvent
            root.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    Intent intent = new Intent(context, ViewEvent.class);
                    intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                    Bundle b = new Bundle();
                    b.putParcelable("badEvent", badEvent);
                    intent.putExtras(b);
                    context.startActivity(intent);
                }
            });
        }
    }
}