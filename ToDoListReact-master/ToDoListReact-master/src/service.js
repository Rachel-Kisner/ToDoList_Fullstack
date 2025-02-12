import axios from 'axios';

const apiUrl = process.env.REACT_APP_API_URL;
axios.defaults.baseURL = apiUrl;
console.log(axios.defaults.baseURL);

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

  // addTask: async(name)=>{
  //   const result = await axios.post(`${apiUrl}/tasks`, {Name:name,IsComplete:false})
  //   console.log('addTask-client', name)
  //   return result.data;
  // },
  addTask: async (name) => {
    try {
      const result = await apiClient.post(`/tasks`, { name ,isComplete:false});
      console.log('addTask', result.data);
      return result.data;
    } catch (error) {
      console.error('Error in addTask:', error.message);
      return {};
    }
  },
  setCompleted: async(id, isComplete, name)=>{
    const result = await axios.put(`${apiUrl}/tasks/${id}?IsComplete=${isComplete}`, {
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
