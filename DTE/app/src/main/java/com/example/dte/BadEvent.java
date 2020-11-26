package com.example.dte;

public class BadEvent {
   private int id;
   private String message;
   private String reason;
   private String placement;
   private String date;
   private int severityId;
   private int statusId;

    public BadEvent() {
    }

    public BadEvent(int id, String message, String reason, String placement, String date, int severityId, int statusId) {
        this.id = id;
        this.message = message;
        this.reason = reason;
        this.placement = placement;
        this.date = date;
        this.severityId = severityId;
        this.statusId = statusId;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getReason() {
        return reason;
    }

    public void setReason(String reason) {
        this.reason = reason;
    }

    public String getPlacement() {
        return placement;
    }

    public void setPlacement(String placement) {
        this.placement = placement;
    }

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }

    public int getSeverityId() {
        return severityId;
    }

    public void setSeverityId(int severityId) {
        this.severityId = severityId;
    }

    public int getStatusId() {
        return statusId;
    }

    public void setStatusId(int statusId) {
        this.statusId = statusId;
    }
}