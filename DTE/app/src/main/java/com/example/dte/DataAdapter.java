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

<<<<<<< HEAD
    private LayoutInflater inflater;
    private List<BadEvent> event;

    DataAdapter(Context context, List<BadEvent> event) {
        System.out.println(event);
        this.event = event;
        this.inflater = LayoutInflater.from(context);
    }

=======
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

>>>>>>> 2f8af475c8f1fae6b10ac51856a78cec073cfb9e
    @Override
    public DataAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        return new DataAdapter.ViewHolder(LayoutInflater.from(parent.getContext()).inflate(R.layout.list_item, parent, false));
    }

    @Override
    public void onBindViewHolder(DataAdapter.ViewHolder holder, int position) {
<<<<<<< HEAD
        final BadEvent events = event.get(position);
        holder.nameView.setText(events.getPlacement());
        holder.timeView.setText(events.getDate());
        holder.alvorlighetsgradView.setText(events.getStatusId()==1?Integer.toString(events.getSeverityId()): "Grad");

=======
        holder.onBind(position);
>>>>>>> 2f8af475c8f1fae6b10ac51856a78cec073cfb9e
    }


    @Override
    public int getItemCount() {
        return events.size();
    }

<<<<<<< HEAD
    public class ViewHolder extends RecyclerView.ViewHolder implements View.OnClickListener {
        final TextView nameView, timeView, alvorlighetsgradView;
=======
    public class ViewHolder extends RecyclerView.ViewHolder {
        TextView nameView, timeView, alvorlighetsgradView;
        LinearLayout root;
>>>>>>> 2f8af475c8f1fae6b10ac51856a78cec073cfb9e
        ViewHolder(View view){
            super(view);
            root = (LinearLayout) view.findViewById(R.id.rootLinearLayout);
            nameView = (TextView) view.findViewById(R.id.name);
            timeView = (TextView) view.findViewById(R.id.time);
            alvorlighetsgradView = (TextView) view.findViewById(R.id.alvorlighetsgrad);
            itemView.setOnClickListener(this);
        }

        @Override
        public void onClick(View v) {
            if (mItemClickListener != null)
                mItemClickListener.onItemClickListener(getAdapterPosition(), v);

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

    public void setOnItemClickListener(ItemClickListener itemClick) {
        this.mItemClickListener = itemClick;

    }



    public ItemClickListener mItemClickListener;



    public interface ItemClickListener {
        public void onItemClickListener(int pos, View v);
    }


}