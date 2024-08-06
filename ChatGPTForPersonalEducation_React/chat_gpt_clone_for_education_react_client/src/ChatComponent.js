import React, { useState } from 'react';

const ChatComponent = () => {
    const [conversationId, setConversationId] = useState('');
    const [message, setMessage] = useState('');
    const [response, setResponse] = useState(null);

    const handleSendMessage = async () => {
        const requestBody = {
            conversationId: conversationId,
            message: message
        };

        try {
            const res = await fetch('https://localhost:7171/api/continue_session', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(requestBody)
            });
            const data = await res.json();
            setResponse(data);
        } catch (error) {
            console.error('Error:', error);
        }
    };

    return (
        <div>
            <h1>Chat Application</h1>
            <input
                type="text"
                placeholder="Conversation ID"
                value={conversationId}
                onChange={(e) => setConversationId(e.target.value)}
            />
            <input
                type="text"
                placeholder="Message"
                value={message}
                onChange={(e) => setMessage(e.target.value)}
            />
            <button onClick={handleSendMessage}>Send Message</button>
            {response && (
                <div>
                    <h2>Response</h2>
                    <pre>{JSON.stringify(response, null, 2)}</pre>
                </div>
            )}
        </div>
    );
};

export default ChatComponent;