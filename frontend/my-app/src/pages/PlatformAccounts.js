import { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import PlatformForm from '../components/platforms/PlatformForm';
import platformsAPI from '../api/platforms';

const PlatformAccounts = () => {
  const { user } = useAuth();
  const [platforms, setPlatforms] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchPlatforms = async () => {
      try {
        const token = user.token;
        const data = await platformsAPI.getUserPlatforms(token);
        setPlatforms(data);
      } catch (err) {
        setError(err.response?.data?.message || 'Failed to fetch platforms');
      } finally {
        setLoading(false);
      }
    };
    fetchPlatforms();
  }, [user]);

  const handlePlatformAdded = async () => {
    try {
      const token = user.token;
      const data = await platformsAPI.getUserPlatforms(token);
      setPlatforms(data);
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to refresh platforms');
    }
  };

  const handleRemovePlatform = async (id) => {
    try {
      const token = user.token;
      await platformsAPI.removePlatform(id, token);
      setPlatforms(platforms.filter(platform => platform.id !== id));
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to remove platform');
    }
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div>
      <h1>Connected Platforms</h1>
      <div>
        <h2>Add New Platform</h2>
        <PlatformForm onPlatformAdded={handlePlatformAdded} />
      </div>
      <div>
        <h2>Your Platforms</h2>
        {platforms.length > 0 ? (
          <ul>
            {platforms.map((platform) => (
              <li key={platform.id}>
                <h3>{platform.platformType}</h3>
                <p>Account ID: {platform.accountId}</p>
                <button onClick={() => handleRemovePlatform(platform.id)}>
                  Remove
                </button>
              </li>
            ))}
          </ul>
        ) : (
          <p>No platforms connected yet</p>
        )}
      </div>
    </div>
  );
};

export default PlatformAccounts;