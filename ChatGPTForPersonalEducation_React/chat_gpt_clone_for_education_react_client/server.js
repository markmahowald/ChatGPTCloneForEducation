const express = require('express');
const cors = require('cors');
const app = express();
const PORT = process.env.PORT || 5000;

// Enable CORS
app.use(cors({
    origin: 'http://localhost:4000', // React app's URL
    methods: 'GET, POST, PUT, DELETE, OPTIONS', // Allowed methods
    allowedHeaders: 'Content-Type, Authorization' // Allowed headers
}));

// Dummy endpoint for testing
app.get('/api/test', (req, res) => {
    res.json({ message: 'Hello from the server!' });
});

app.listen(PORT, () => {
    console.log(`Server is running on port ${PORT}`);
});
