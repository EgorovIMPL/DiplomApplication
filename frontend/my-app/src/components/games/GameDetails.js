import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import gamesAPI from '../../api/games';

const GameDetails = () => {
  const { id } = useParams();
  const [game, setGame] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchGame = async () => {
      try {
        const data = await gamesAPI.getGameById(id);
        setGame(data);
      } catch (err) {
        setError(err.response?.data?.message || 'Failed to fetch game');
      } finally {
        setLoading(false);
      }
    };
    fetchGame();
  }, [id]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;
  if (!game) return <div>Game not found</div>;

  return (
    <div className="game-details">
      <h2>{game.title}</h2>
      <img src={game.image} alt={game.title} />
      <p>{game.description}</p>
      <p>Genre: {game.genre}</p>
      <p>Platform: {game.platform}</p>
      <p>Release Date: {new Date(game.releaseDate).toLocaleDateString()}</p>
    </div>
  );
};

export default GameDetails;