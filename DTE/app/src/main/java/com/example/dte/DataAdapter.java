package com.example.dte;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.recyclerview.widget.RecyclerView;

import java.util.List;

class DataAdapter extends RecyclerView.Adapter<DataAdapter.ViewHolder> {

    private LayoutInflater inflater;
    private List<Events> event;

    DataAdapter(Context context, List<Events> event) {
        this.event = event;
        this.inflater = LayoutInflater.from(context);
    }
    @Override
    public DataAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        View view = inflater.inflate(R.layout.list_item, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(DataAdapter.ViewHolder holder, int position) {
        Events events = event.get(position);
        holder.nameView.setText(events.getName());
        holder.timeView.setText(events.getTime());
        holder.alvorlighetsgradView.setText(events.getAlvorlighetsgrad());

    }

    @Override
    public int getItemCount() {
        return event.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        final TextView nameView, timeView, alvorlighetsgradView;
        ViewHolder(View view){
            super(view);
            nameView = (TextView) view.findViewById(R.id.name);
            timeView = (TextView) view.findViewById(R.id.time);
            alvorlighetsgradView = (TextView) view.findViewById(R.id.alvorlighetsgrad);
        }
    }
}