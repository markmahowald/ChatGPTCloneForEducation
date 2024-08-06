import React, { useState, useEffect } from 'react';
import './App.css';
import './normal.css';
import axios from 'axios';

function App() {
  const apiUrl = process.env.REACT_APP_API_URL;
  const [conversationId, setConversationId] = useState('');
  const [message, setMessage] = useState('');
  const [response, setResponse] = useState(null);
  const [sessions, setSessions] = useState([]);
  const [selectedSession, setSelectedSession] = useState(null);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    retrieveAllSessions();
  }, []);

  const handleSendMessage = async () => {
    setLoading(true);
    setError(null);

    if (!conversationId) {
        // Create a new session
        const requestBody = {
            initialMessage: message
        };

        try {
            console.log('Sending new session request to:', `${apiUrl}/api/new_session`, 'with body:', requestBody);
            const res = await axios.post(`${apiUrl}/api/new_session`, requestBody, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            const data = res.data;
            console.log('New Session Response:', data);

            if (!data.conversationId) {
                throw new Error('Invalid response from server: conversationId not found');
            }

            setConversationId(data.conversationId); // Set the new conversation ID
            setSelectedSession(data); // Set the initial messages
            await retrieveAllSessions(); // Refresh the list of sessions
            setMessage(''); // Clear the input field
        } catch (error) {
            console.error('Error creating new session:', error.response ? error.response.data : error.message);
            setError('Failed to create new session.');
        } finally {
            setLoading(false);
        }
    } else {
        // Continue existing session
        const requestBody = {
            conversationId: conversationId,
            message: message
        };

        try {
            const res = await axios.post(`${apiUrl}/api/continue_session`, requestBody, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            console.log('Send Message Response:', res.data);
            setResponse(res.data);
            await retrieveSessionById(conversationId); // Refresh the session messages
            setMessage(''); // Clear the input field
        } catch (error) {
            console.error('Error sending message:', error.response ? error.response.data : error.message);
            setError('Failed to send message.');
        } finally {
            setLoading(false);
        }
    }
};

  const retrieveAllSessions = async () => {
    setLoading(true);
    try {
      console.log(`${apiUrl}/api/retrieve_sessions/retrieve_all_sessions`)
      const res = await axios.get(`${apiUrl}/api/retrieve_sessions/retrieve_all_sessions`);
      console.log('All Sessions:', res.data);
      setSessions(res.data);
    } catch (error) {
      console.error('Error retrieving all sessions:', error);
      setError('Failed to retrieve sessions.');
    } finally {
      setLoading(false);
    }
  };

  const retrieveSessionById = async (id) => {
    setLoading(true);
    try {
      const res = await axios.get(`${apiUrl}/api/retrieve_sessions/${id}`);
      console.log('Session by ID:', res.data);
      setSelectedSession(res.data); // Assuming res.data is an array of messages
      setConversationId(id);
    } catch (error) {
      console.error('Error retrieving session by ID:', error);
      setError('Failed to retrieve session by ID.');
    } finally {
      setLoading(false);
    }
  };

  const handleNewSession = async () => {
    setConversationId('');
    setMessage('');
    setSelectedSession(null);
    setError(null);
};


  return (
    <div className="App">
      <aside className="sidemenu">
        <div className="side-menu-button" onClick={handleNewSession}>
          <span>+</span>
          New Chat 
        </div>
        <div className="session-list">
          {sessions.map(session => (
            <div
              key={session.conversation.id}
              className={`session-item ${session.conversation.id === conversationId ? 'selected' : ''}`}
              onClick={() => retrieveSessionById(session.conversation.id)}
            >
              {session.conversation.topicDescription ? ` ${session.conversation.topicDescription}` : `Session: ${session.conversation.id}`}
            </div>
          ))}
        </div>
      </aside>
      <section className="chatbox">
        <div className="chat-log">
          {selectedSession && selectedSession.messages.map((msg, index) => (
            <div key={index} className={`chat-message ${msg.role === 'user' ? '' : 'chatgpt'}`}>
              <div className="chat-message-center">
                <div className={`avatar ${msg.role === 'user' ? '' : 'chatgpt'}`}>
                  {msg.role === 'user' ? <span role="img" aria-label="user">👤</span> : <span role="img" aria-label="assistant">🤖</span>}
                </div>
                <div className="message">
                  {msg.content}
                </div>
              </div>
            </div>
          ))}
        </div>
        <div className="chat-input-holder">
          <textarea 
            className="chat-input-text-area"
            placeholder="Type your input here!"
            rows="1"
            value={message}
            onChange={(e) => setMessage(e.target.value)}
            disabled={loading}
          />
          <button onClick={handleSendMessage} disabled={loading}>
            {loading ? 'Sending...' : 'Send'}
          </button>
        </div>
        {error && (
          <div className="error-message">
            {error}
          </div>
        )}
      </section>
    </div>
  );
}

export default App;
