package com.markmahowald.SharedClassesAndUtilities;



public class Message {
    private String role;
    private String content;

    // Default constructor
public Message() {
    super();
}

    // Constructor with parameters
    public Message(String role, String content) {
        super();
        this.role = role;
        this.content = content;
    }

    // Getter and setter for role
    public String getRole() {
        return role;
    }

    public void setRole(String role) {
        this.role = role;
    }

    // Getter and setter for content
    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }
}
