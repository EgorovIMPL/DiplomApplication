import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import gamesAPI from '../api/games';
import GameCard from '../components/games/GameCard';

const Games = () => {
  const { platformAccountId } = useParams();
  const { user } = useAuth();
  const [games, setGames] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  
    const fetchGames = async () => {
      try {
        const token = user.token;
        const data = await gamesAPI.getGamesByPlatform(platformAccountId, token);
        setGames(data);
      } catch (err) {
        setError(err.response?.data?.message || 'Failed to fetch games');
      } finally {
        setLoading(false);
      }
    };

  const handleSyncGames = async () => {
    try {
      const token = user.token;
      // Здесь нужно получить platformType и accountId для выбранного platformAccountId
      // Это может потребовать дополнительного запроса или хранения этой информации в состоянии
      await gamesAPI.syncGames(platformAccountId, 'Steam', 'account123', token);
      // После синхронизации обновляем список игр
      const data = await gamesAPI.getGamesByPlatform(platformAccountId, token);
      setGames(data);
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to sync games');
    }
  };

  if (loading) return <div><button onClick={fetchGames}>Get Games</button></div>;
  if (error) return <div>{error}</div>;

  return (
    <div>
      <h1>Games</h1>
      <button onClick={handleSyncGames}>Sync Games</button>
      <div className="games-grid">
        {games.map((game) => (
          <GameCard key={game.id} game={game} />
        ))}
      </div>
    </div>
  );
};

export default Games;