import axios from 'axios';

const API_URL = 'https://localhost:44398/api/Auth';

const register = async (userData) => {
  const response = await axios.post(`${API_URL}/register`, {
    username: userData.username,
    email: userData.email,
    password: userData.password,
    displayName: userData.username // Используем username как displayName, если не предусмотрено отдельное поле
  });
  return response.data;
};

const login = async (userData) => {
  const response = await axios.post(`${API_URL}/login`, {
    username: userData.username,
    password: userData.password
  });
  
  if (response.data.token) {
    localStorage.setItem('user', JSON.stringify({
      token: response.data.token,
      username: userData.username,
      expires: response.data.expires
    }));
  }
  return response.data;
};

const getUserDetails = async (userId) => {
  const response = await axios.get(`https://localhost:44398/api/User/user/${userId}`, {
    username: response.data.username,
    email: response.data.email,
    displayName: response.data.displayName
  });
  return response.data;
}

const logout = () => {
  localStorage.removeItem('user');
};

const authAPI = {
  register,
  login,
  logout,
  getUserDetails
};

export default authAPI;