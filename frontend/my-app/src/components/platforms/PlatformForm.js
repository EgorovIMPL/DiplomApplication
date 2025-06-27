import { useState } from 'react';
import platformsAPI from '../../api/platforms';

const PlatformForm = ({ onPlatformAdded }) => {
  const [platformData, setPlatformData] = useState({
    platformType: '',
    accountId: '',
    credentials: {
      username: '',
      password: ''
    }
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    if (name in platformData.credentials) {
      setPlatformData({
        ...platformData,
        credentials: {
          ...platformData.credentials,
          [name]: value
        }
      });
    } else {
      setPlatformData({
        ...platformData,
        [name]: value
      });
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      const token = JSON.parse(localStorage.getItem('user')).token;
      await platformsAPI.addPlatform(platformData, token);
      onPlatformAdded();
      setPlatformData({
        platformType: '',
        accountId: '',
        credentials: {
          username: '',
          password: ''
        }
      });
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to add platform');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      {error && <div className="error">{error}</div>}
      <div>
        <label>Platform Type</label>
        <select
          name="platformType"
          value={platformData.platformType}
          onChange={handleChange}
          required
        >
          <option value="">Select Platform</option>
          <option value="Steam">Steam</option>
          <option value="Epic">Epic Games</option>
          <option value="Origin">Origin</option>
          <option value="Xbox">Xbox</option>
          <option value="PlayStation">PlayStation</option>
        </select>
      </div>
      <div>
        <label>Account ID</label>
        <input
          type="text"
          name="accountId"
          value={platformData.accountId}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label>Username</label>
        <input
          type="text"
          name="username"
          value={platformData.credentials.username}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label>Password</label>
        <input
          type="password"
          name="password"
          value={platformData.credentials.password}
          onChange={handleChange}
          required
        />
      </div>
      <button type="submit" disabled={loading}>
        {loading ? 'Adding...' : 'Add Platform'}
      </button>
    </form>
  );
};

export default PlatformForm;