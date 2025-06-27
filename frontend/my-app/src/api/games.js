import axios from 'axios';

const API_URL = 'https://localhost:44398/api/games';

const getGamesByPlatform = async (platformAccountId, token) => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };
  const response = await axios.get(`${API_URL}/platform/${platformAccountId}`, config);
  return response.data;
};

const syncGames = async (platformAccountId, platformType, accountId, token) => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
    params: {
      platformType,
      accountId
    }
  };
  await axios.post(`${API_URL}/sync/${platformAccountId}`, null, config);
};

const gamesAPI = {
  getGamesByPlatform,
  syncGames,
};

export default gamesAPI;