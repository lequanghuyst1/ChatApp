import React from 'react';
import { useForm } from 'react-hook-form';

interface LoginFormProps {
  onSwitchToRegister: () => void;
}

interface LoginFormInputs {
  email: string;
  password: string;
}

const LoginForm: React.FC<LoginFormProps> = ({ onSwitchToRegister }) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormInputs>();

  const onSubmit = (data: LoginFormInputs) => {
    // Handle login logic here
    console.log('Login:', data);
  };

  return (
    <div className="auth-form-container">
      <h2>Login</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div>
          <label>Email</label>
          <input
            {...register('email', {
              required: 'Email is required',
              pattern: {
                value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                message: 'Invalid email address',
              },
            })}
            type="email"
            placeholder="Enter your email"
          />
          {errors.email && <span className="error">{errors.email.message}</span>}
        </div>
        <div>
          <label>Password</label>
          <input
            {...register('password', { required: 'Password is required' })}
            type="password"
            placeholder="Enter your password"
          />
          {errors.password && <span className="error">{errors.password.message}</span>}
        </div>
        <button type="submit">Login</button>
      </form>
      <p>
        Don't have an account?{' '}
        <button type="button" onClick={onSwitchToRegister} className="switch-btn">
          Register
        </button>
      </p>
    </div>
  );
};

export default LoginForm;
