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
    private List<BadEvent> event;

    DataAdapter(Context context, List<BadEvent> event) {
        System.out.println(event);
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
        final BadEvent events = event.get(position);
        holder.nameView.setText(events.getPlacement());
        holder.timeView.setText(events.getDate());
        holder.alvorlighetsgradView.setText(events.getStatusId()==1?Integer.toString(events.getSeverityId()): "Grad");

    }


    @Override
    public int getItemCount() {
        return event.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder implements View.OnClickListener {
        final TextView nameView, timeView, alvorlighetsgradView;
        ViewHolder(View view){
            super(view);
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
    }

    public void setOnItemClickListener(ItemClickListener itemClick) {
        this.mItemClickListener = itemClick;

    }



    public ItemClickListener mItemClickListener;



    public interface ItemClickListener {
        public void onItemClickListener(int pos, View v);
    }


}