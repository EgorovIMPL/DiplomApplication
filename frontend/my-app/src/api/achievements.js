import axios from 'axios';

const API_URL = 'https://localhost:44398/api/api/achievements';

const getAchievementsByGame = async (gameId, token) => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };
  const response = await axios.get(`${API_URL}/game/${gameId}`, config);
  return response.data;
};

const syncAchievements = async (gameId, platformType, accountId, token) => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
    params: {
      platformType,
      accountId
    }
  };
  await axios.post(`${API_URL}/sync/${gameId}`, null, config);
};

const achievementsAPI = {
  getAchievementsByGame,
  syncAchievements,
};

export default achievementsAPI;