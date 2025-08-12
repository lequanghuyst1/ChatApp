import React from 'react';
import { useForm } from 'react-hook-form';

interface RegisterFormProps {
  onSwitchToLogin: () => void;
}

interface RegisterFormInputs {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
}

const RegisterForm: React.FC<RegisterFormProps> = ({ onSwitchToLogin }) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
    watch,
  } = useForm<RegisterFormInputs>();

  const onSubmit = (data: RegisterFormInputs) => {
    // Handle registration logic here
    console.log('Register:', data);
  };

  const password = watch('password');

  return (
    <div className="auth-form-container">
      <h2>Register</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div>
          <label>Username</label>
          <input
            {...register('username', { required: 'Username is required' })}
            type="text"
            placeholder="Enter your username"
          />
          {errors.username && <span className="error">{errors.username.message}</span>}
        </div>
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
            {...register('password', { required: 'Password is required', minLength: { value: 6, message: 'Password must be at least 6 characters' } })}
            type="password"
            placeholder="Enter your password"
          />
          {errors.password && <span className="error">{errors.password.message}</span>}
        </div>
        <div>
          <label>Confirm Password</label>
          <input
            {...register('confirmPassword', {
              required: 'Please confirm your password',
              validate: value => value === password || 'Passwords do not match',
            })}
            type="password"
            placeholder="Confirm your password"
          />
          {errors.confirmPassword && <span className="error">{errors.confirmPassword.message}</span>}
        </div>
        <button type="submit">Register</button>
      </form>
      <p>
        Already have an account?{' '}
        <button type="button" onClick={onSwitchToLogin} className="switch-btn">
          Login
        </button>
      </p>
    </div>
  );
};

export default RegisterForm;
