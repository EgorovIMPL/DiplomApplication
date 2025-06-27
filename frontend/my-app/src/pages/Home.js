import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Home = () => {
  const { user } = useAuth();

  return (
    <div>
      <h1>Welcome to GameTracker</h1>
      {user ? (
        <p>
          Welcome back, {user.username}! <Link to="/dashboard">Go to Dashboard</Link>
        </p>
      ) : (
        <div>
          <p>Track your gaming progress and achievements</p>
          <div>
            <Link to="/login">Login</Link> or <Link to="/register">Register</Link>
          </div>
        </div>
      )}
    </div>
  );
};

export default Home;