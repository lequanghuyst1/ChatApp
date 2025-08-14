import { Box, Button, Stack, TextField, Typography } from "@mui/material";
import React from "react";
import { Controller, useForm } from "react-hook-form";
import { useAuthContext } from "../../stores/auth";
import { LoadingButton } from "@mui/lab";
import { ILoginRequest } from "../../types/account";

interface LoginFormProps {
  onSwitchToRegister: () => void;
}

const LoginForm: React.FC<LoginFormProps> = ({ onSwitchToRegister }) => {
  const {
    control,
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ILoginRequest>();

  const { login } = useAuthContext();

  const [loading, setLoading] = React.useState(false);
  const [messageError, setMessageError] = React.useState("");

  const onSubmit = async (data: ILoginRequest) => {
    try {
      setLoading(true);
      console.log("Login:", data);
      await login(data);
    } catch (error: any) {
      console.error("Login failed:", error);
      setMessageError(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box sx={{ width: "100%" }}>
      <Typography sx={{ mb: 2, textAlign: "center" }} variant="h5">
        Login
      </Typography>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Stack spacing={2}>
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
            Login
          </LoadingButton>
        </Stack>
      </form>
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
          mt: 2,
        }}
      >
        <Typography variant="body2">Don't have an account?</Typography>
        <Button onClick={onSwitchToRegister} variant="outlined" sx={{ mt: 0 }}>
          Register
        </Button>
      </Box>
    </Box>
  );
};

export default LoginForm;
