import axios from 'axios';
// REACT_APP_API_URL=https://authserver-kmei.onrender.com
const apiUrl = process.env.REACT_APP_API_URL;
axios.defaults.baseURL = apiUrl;
console.log("axios url"+axios.defaults.baseURL);

axios.interceptors.response.use(
  response => response,
  error => {
    console.error("API Error:", error.response ? error.response.data : error.message);
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
    
    const result = await axios.get(`${apiUrl}/tasks`);   
    console.log(`${apiUrl}/tasks`);
     
    console.log("client-get");
    return result.data;
    
  },

  addTask: async(item)=>{
    const result = await axios.post(`${apiUrl}/tasks`, {item})
    console.log('addTask-client', item)
    return result.data;
  },

  setCompleted: async(id, isComplete, name)=>{
    const result = await axios.put(`${apiUrl}/tasks/${id}`, {
      isComplete,
      Name:name
    });
    console.log("client-put");
    return result.data;
  },

  deleteTask:async(id)=>{
    const result = await axios.delete(`/tasks/${id}`)
    console.log('deleteTask-server', id)
    return result.data;
  }
};
