import { useState } from 'react';
import { Card, CardContent, Grid, Stack } from '@mui/material';
import LoginForm from '../login-form';
import RegisterForm from '../register-form';

function JwtLoginView() {
  const [isLogin, setIsLogin] = useState(true);

  return (
    <Stack
      sx={{
        width: '100%',
        height: '100vh',
        background:
          'url(https://cdn.pixabay.com/photo/2019/12/17/16/52/plum-flower-4702008_640.jpg)',
        backgroundSize: 'cover',
        backgroundPosition: 'center',
      }}
      justifyContent="center"
      alignItems="center"
    >
      <Grid container sx={{ justifyContent: 'center' }}>
        <Grid item xs={12} md={5} lg={4} xl={3}>
          <Card>
            <CardContent>
              {isLogin ? (
                <LoginForm onSwitchToRegister={() => setIsLogin(false)} />
              ) : (
                <RegisterForm onSwitchToLogin={() => setIsLogin(true)} />
              )}
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Stack>
  );
}

export default JwtLoginView;
