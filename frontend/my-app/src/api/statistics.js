import axios from 'axios';

const API_URL = 'https://localhost:44398/api/api/statistics';

const getStatisticsByGame = async (gameId, token) => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };
  const response = await axios.get(`${API_URL}/game/${gameId}`, config);
  return response.data;
};

const syncStatistics = async (gameId, platformType, accountId, token) => {
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
    params: {
      platformType,
      accountId
    }
  };
  const response = await axios.post(`${API_URL}/sync/${gameId}`, null, config);
  return response.data;
};

const statisticsAPI = {
  getStatisticsByGame,
  syncStatistics,
};

export default statisticsAPI;