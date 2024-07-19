package com.markmahowald.SharedClassesAndUtilities;



public class OpenAiApiBody {
    private String model;
    private int maxTokens;
    private Conversation conversation;

    // Default constructor
    public OpenAiApiBody() {
        super();
    }

    // Constructor with parameters
    public OpenAiApiBody(String model, int maxTokens, Conversation conversation) {
        super();
        this.model = model;
        this.maxTokens = maxTokens;
        this.conversation = conversation;
    }

    // Getter and setter for model
    public String getModel() {
        return model;
    }

    public void setModel(String model) {
        this.model = model;
    }

    // Getter and setter for maxTokens
    public int getMaxTokens() {
        return maxTokens;
    }

    public void setMaxTokens(int maxTokens) {
        this.maxTokens = maxTokens;
    }

    // Getter and setter for conversation
    public Conversation getConversation() {
        return conversation;
    }

    public void setConversation(Conversation conversation) {
        this.conversation = conversation;
    }
}
