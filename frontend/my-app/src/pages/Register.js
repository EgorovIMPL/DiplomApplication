import RegisterForm from '../components/auth/RegisterForm.js';
import { useAuth } from '../context/AuthContext';
import { Navigate } from 'react-router-dom';

const Register = () => {
  const { user } = useAuth();

  if (user) {
    return <Navigate to="/dashboard" />;
  }

  return (
    <div>
      <h1>Register</h1>
      <RegisterForm />
    </div>
  );
};

export default Register;