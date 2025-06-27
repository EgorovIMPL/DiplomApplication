import { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import statisticsAPI from '../api/statistics';
import achievementsAPI from '../api/achievements';
import GameCard from '../components/games/GameCard';

const Dashboard = () => {
  const { user } = useAuth();
  const [stats, setStats] = useState(null);
  const [achievements, setAchievements] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const token = user.token;
        const [statsData, achievementsData] = await Promise.all([
          statisticsAPI.getUserStatistics(token),
          achievementsAPI.getUserAchievements(token),
        ]);
        setStats(statsData);
        setAchievements(achievementsData);
      } catch (err) {
        setError(err.response?.data?.message || 'Failed to fetch data');
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [user]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div>
      <h1>Dashboard</h1>
      <section>
        <h2>Statistics</h2>
        {stats && (
          <div>
            <p>Games Played: {stats.gamesPlayed}</p>
            <p>Hours Played: {stats.hoursPlayed}</p>
            <p>Achievements Unlocked: {stats.achievementsUnlocked}</p>
          </div>
        )}
      </section>
      <section>
        <h2>Recent Achievements</h2>
        {achievements.length > 0 ? (
          <ul>
            {achievements.slice(0, 5).map((achievement) => (
              <li key={achievement._id}>
                <h3>{achievement.name}</h3>
                <p>{achievement.description}</p>
                <p>Earned on: {new Date(achievement.earnedDate).toLocaleDateString()}</p>
              </li>
            ))}
          </ul>
        ) : (
          <p>No achievements yet</p>
        )}
      </section>
    </div>
  );
};

export default Dashboard;