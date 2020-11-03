package com.example.dte;

public class Events {

    private String name;
    private String time;
    private String alvorlighetsgrad;

    public Events(String name, String time, String alvorlighetsgrad){

        this.name=name;
        this.time = time;
        this.alvorlighetsgrad = alvorlighetsgrad;
    }

    public String getName() {
        return this.name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getTime() {
        return this.time;
    }

    public void setTime(String time) {
        this.time = time;
    }

    public String getAlvorlighetsgrad() {
        return this.alvorlighetsgrad;
    }

    public void setAlvorlighetsgrad(String alvorlighetsgrad) {
        this.alvorlighetsgrad = alvorlighetsgrad;
    }
}