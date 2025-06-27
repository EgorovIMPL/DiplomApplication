const ErrorAlert = ({ message, onClose }) => {
  return (
    <div className="error-alert">
      <span>{message}</span>
      <button onClick={onClose}>&times;</button>
    </div>
  );
};

export default ErrorAlert;