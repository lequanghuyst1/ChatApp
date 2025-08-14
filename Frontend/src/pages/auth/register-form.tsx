import React, { useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { IRegisterRequest } from "../../types/account";
import { useAuthContext } from "../../stores/auth";
import { Box, Button, Stack, TextField, Typography } from "@mui/material";
import { LoadingButton } from "@mui/lab";

interface RegisterFormProps {
  onSwitchToLogin: () => void;
}

const RegisterForm: React.FC<RegisterFormProps> = ({ onSwitchToLogin }) => {
  const {
    control,
    handleSubmit,
    formState: { errors },
    watch,
  } = useForm<IRegisterRequest>();

  const { register } = useAuthContext();

  const [loading, setLoading] = useState(false);
  const [messageError, setMessageError] = useState("");

  const onSubmit = (data: IRegisterRequest) => {
    try {
      setLoading(true);
      console.log("Register:", data);
      register(data);
    } catch (error: any) {
      console.error("Register failed:", error);
      setMessageError(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <Typography variant="h5" gutterBottom sx={{ textAlign: "center" }}>
        Register
      </Typography>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Stack spacing={2}>
          <Controller
            name="firstName"
            control={control}
            rules={{ required: "First name is required" }}
            render={({ field }) => (
              <TextField
                {...field}
                label="First name"
                fullWidth
                error={!!errors.firstName}
                helperText={errors.firstName?.message}
                sx={{
                  borderRadius: 1,
                }}
              />
            )}
          />

          <Controller
            name="lastName"
            control={control}
            rules={{ required: "Last name is required" }}
            render={({ field }) => (
              <TextField
                {...field}
                label="Last name"
                fullWidth
                error={!!errors.lastName}
                helperText={errors.lastName?.message}
                sx={{
                  borderRadius: 1,
                }}
              />
            )}
          />

          <Controller
            name="username"
            control={control}
            rules={{ required: "Username is required" }}
            render={({ field }) => (
              <TextField
                {...field}
                label="Username"
                fullWidth
                error={!!errors.username}
                helperText={errors.username?.message}
                sx={{
                  borderRadius: 1,
                }}
              />
            )}
          />
          <Controller
            name="password"
            control={control}
            rules={{ required: "Password is required" }}
            render={({ field }) => (
              <TextField
                {...field}
                label="Password"
                type="password"
                fullWidth
                error={!!errors.password}
                helperText={errors.password?.message}
                sx={{
                  borderRadius: 2,
                }}
              />
            )}
          />
          <LoadingButton
            loading={loading}
            color="primary"
            variant="contained"
            type="submit"
          >
            Register
          </LoadingButton>
        </Stack>
      </form>

      <Box sx={{ display: "flex", alignItems: "center", mt: 2, gap: 1 }}>
        <Typography variant="body2"> Already have an account?</Typography>
        <Typography
          variant="body2"
          sx={{ cursor: "pointer", color: "primary.main" }}
          onClick={onSwitchToLogin}
        >
          Login
        </Typography>
      </Box>
    </div>
  );
};

export default RegisterForm;
