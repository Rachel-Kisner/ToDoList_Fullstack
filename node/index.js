const express = require('express');
const axios = require('axios');

const app = express();
const PORT = 3000;

const API_KEY = "rnd_5COMC1wd3gzJvWz7MyjwN9jkuBUK"; 
const API_URL = "https://api.render.com/v1/services";

app.get('/services', async (req, res) => {
    try {
        const response = await axios.get(API_URL, {
            headers: {
                'Authorization': `Bearer ${API_KEY}`,
                'Accept': 'application/json'
            }
        });
        res.json(response.data);
    } catch (error) {
        console.error("Error fetching services:", error.response?.data || error.message);
        res.status(500).json({ error: "Failed to fetch services" });
    }
});

app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});
