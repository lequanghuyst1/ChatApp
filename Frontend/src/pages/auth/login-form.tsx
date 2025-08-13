import { Box, TextField } from "@mui/material";
import React from "react";
import { Controller, useForm } from "react-hook-form";

interface LoginFormProps {
  onSwitchToRegister: () => void;
}

interface LoginFormInputs {
  username: string;
  password: string;
}

const LoginForm: React.FC<LoginFormProps> = ({ onSwitchToRegister }) => {
  const {
    control,
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormInputs>();

  const onSubmit = (data: LoginFormInputs) => {
    console.log("Login:", data);
  };

  return (
    <Box>
      <h2>Login</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Box>
          <Controller
            name="username"
            control={control}
            rules={{
              required: "Username is required",
            }}
            render={({ field }) => (
              <TextField
                {...field}
                label="Username"
                error={!!errors.username}
                helperText={errors.username?.message}
              />
            )}
          />
        </Box>
        <Box>
          <Controller
            name="password"
            control={control}
            rules={{ required: "Password is required" }}
            render={({ field }) => (
              <TextField
                {...field}
                label="Password"
                type="password"
                error={!!errors.password}
                helperText={errors.password?.message}
              />
            )}
          />
        </Box>
        <button type="submit">Login</button>
      </form>
      <p>
        Don't have an account?{" "}
        <button type="button" onClick={onSwitchToRegister}>
          Register
        </button>
      </p>
    </Box>
  );
};

export default LoginForm;
