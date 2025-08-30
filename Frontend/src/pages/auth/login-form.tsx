import { Box, Stack, TextField, Typography, Alert } from '@mui/material';
import React from 'react';
import { Controller, useForm } from 'react-hook-form';
import { LoadingButton } from '@mui/lab';
import { ILoginRequest } from '@/types/account';
import { useAppDispatch, useAppSelector } from '@/stores/hook';
import { login, clearError } from '@/stores/slices/authSlice';

interface LoginFormProps {
  onSwitchToRegister: () => void;
}

const LoginForm: React.FC<LoginFormProps> = ({ onSwitchToRegister }) => {
  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<ILoginRequest>();

  const dispatch = useAppDispatch();
  const { loading, error } = useAppSelector((state) => state.auth);

  const onSubmit = async (data: ILoginRequest) => {
    try {
      await dispatch(login(data)).unwrap();
    } catch (error) {
      // Error is already handled by the thunk
      console.error('Login failed:', error);
    }
  };

  React.useEffect(() => {
    // Clear any previous errors when the component mounts
    dispatch(clearError());
  }, [dispatch]);

  return (
    <Box sx={{ width: '100%' }}>
      <Typography sx={{ mb: 2, textAlign: 'center' }} variant="h5">
        Login
      </Typography>
      {error && (
        <Alert severity="error" sx={{ mb: 2 }} onClose={() => dispatch(clearError())}>
          {error}
        </Alert>
      )}
      <form onSubmit={handleSubmit(onSubmit)}>
        <Stack spacing={2}>
          <Controller
            name="username"
            control={control}
            rules={{ required: 'Username is required' }}
            render={({ field }) => (
              <TextField
                {...field}
                label="Username"
                required
                fullWidth
                error={!!errors.username}
                helperText={errors.username?.message}
                sx={{ borderRadius: 1 }}
              />
            )}
          />
          <Controller
            name="password"
            control={control}
            rules={{ required: 'Password is required' }}
            render={({ field }) => (
              <TextField
                {...field}
                label="Password"
                required
                type="password"
                fullWidth
                error={!!errors.password}
                helperText={errors.password?.message}
                sx={{ borderRadius: 2 }}
              />
            )}
          />
          <LoadingButton loading={loading} color="primary" variant="contained" type="submit">
            Login
          </LoadingButton>
        </Stack>
      </form>
      <Box sx={{ display: 'flex', alignItems: 'center', mt: 2, gap: 1 }}>
        <Typography variant="body2">Don't have an account?</Typography>
        <Typography
          variant="body2"
          sx={{ cursor: 'pointer', color: 'primary.main' }}
          onClick={onSwitchToRegister}
        >
          Register
        </Typography>
      </Box>
    </Box>
  );
};

export default LoginForm;
