import axios from 'axios';

axios.defaults.baseURL = "http://localhost:5084";

axios.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error("API Error:", error.response ? error.response.data : error.message);
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
    const result = await axios.get('/tasks');    
    return result.data;
    
  },

  addTask: async(item)=>{
    console.log('addTask', item)
    const result = await axios.post('/tasks', item)
    return result.data;
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    const result = await axios.put(`/tasks/${id}`, {isComplete})
    return result.data;
  },

  deleteTask:async(id)=>{
    console.log('deleteTask', id)
    const result = await axios.delete(`/tasks/${id}`)
    return result.data;
  }
};
