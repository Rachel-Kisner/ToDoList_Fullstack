import axios from 'axios';

const apiUrl = process.env.REACT_APP_API_URL;
axios.defaults.baseURL = apiUrl;

axios.interceptors.response.use(
  response => response,
  (error) => {
    console.error("API Error:", error.response ? error.response.data : error.message);
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
    const result = await axios.get(`${apiUrl}/tasks`);    
    return result.data;
    
  },

  addTask: async(item)=>{
    console.log('addTask', item)
    const result = await axios.post(`${apiUrl}/tasks`, {item})
    return result.data;
  },

  setCompleted: async(id, isComplete, name)=>{
    const result = await axios.put(`${apiUrl}/tasks/${id}`, {
      isComplete,
      Name:name
    });
    return result.data;
  },

  deleteTask:async(id)=>{
    console.log('deleteTask', id)
    const result = await axios.delete(`/tasks/${id}`)
    return result.data;
  }
};
