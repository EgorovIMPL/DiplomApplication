import axios from 'axios';

const API_URL = 'https://localhost:44398/api/api/account/platforms';

const getUserPlatforms = async (token) => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };
  const response = await axios.get(API_URL, config);
  return response.data;
};

const addPlatform = async (platformData, token) => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };
  await axios.post(API_URL, platformData, config);
};

const removePlatform = async (id, token) => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };
  await axios.delete(`${API_URL}/${id}`, config);
};

const platformsAPI = {
  getUserPlatforms,
  addPlatform,
  removePlatform,
};

export default platformsAPI;