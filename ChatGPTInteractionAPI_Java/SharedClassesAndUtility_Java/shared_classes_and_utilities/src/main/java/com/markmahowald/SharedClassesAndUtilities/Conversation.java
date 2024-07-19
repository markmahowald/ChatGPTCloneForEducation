package com.markmahowald.SharedClassesAndUtilities;


import java.beans.PropertyChangeListener;
import java.beans.PropertyChangeSupport;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

public class Conversation {
    private List<Message> messages = new ArrayList<>();
    private UUID id;
    private String topicDescription;
    private String model = "gpt-4";
    private int maxTokens = 150;

    private PropertyChangeSupport propertyChangeSupport = new PropertyChangeSupport(this);

    public Conversation() {
        super();
    }

    public List<Message> getMessages() {
        return messages;
    }

    public void setMessages(List<Message> messages) {
        List<Message> oldMessages = this.messages;
        this.messages = messages;
        propertyChangeSupport.firePropertyChange("messages", oldMessages, messages);
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        UUID oldId = this.id;
        this.id = id;
        propertyChangeSupport.firePropertyChange("id", oldId, id);
    }

    public String getModel() {
        return model;
    }

    public int getMaxTokens() {
        return maxTokens;
    }

    public String getTopicDescription() {
        return topicDescription;
    }

    public void setTopicDescription(String topicDescription) {
        String oldTopicDescription = this.topicDescription;
        this.topicDescription = topicDescription;
        propertyChangeSupport.firePropertyChange("topicDescription", oldTopicDescription, topicDescription);
    }

    public void addPropertyChangeListener(PropertyChangeListener listener) {
        propertyChangeSupport.addPropertyChangeListener(listener);
    }

    public void removePropertyChangeListener(PropertyChangeListener listener) {
        propertyChangeSupport.removePropertyChangeListener(listener);
    }

    public void triggerAllPropertyChanged() {
        propertyChangeSupport.firePropertyChange("messages", null, messages);
        propertyChangeSupport.firePropertyChange("id", null, id);
        propertyChangeSupport.firePropertyChange("model", null, model);
        propertyChangeSupport.firePropertyChange("maxTokens", null, maxTokens);
        propertyChangeSupport.firePropertyChange("topicDescription", null, topicDescription);
    }

    public void addMessage(String role, String content) {
        Message message = new Message(role, content);
        messages.add(message);
        propertyChangeSupport.firePropertyChange("messages", null, messages);
    }

    public OpenAiApiBody generateOpenAiBody() {
        OpenAiApiBody result = new OpenAiApiBody();
        result.setModel(this.model);
        result.setMaxTokens(this.maxTokens);
        result.setConversation(this);
        return result;
    }
}
