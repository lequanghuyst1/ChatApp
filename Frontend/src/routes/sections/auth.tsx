import { Suspense, lazy } from 'react';
import { Outlet } from 'react-router';
import { SplashScreen } from '@/components/loading-screen';
import { GuestGuard } from '@/components/guard';

const JwtLoginPage = lazy(() => import('@/pages/auth/views/jwt-login-view'));

export const authRoutes = [
  {
    element: (
      <GuestGuard>
        <Suspense fallback={<SplashScreen />}>
          <Outlet />
        </Suspense>
      </GuestGuard>
    ),
    children: [
      {
        path: 'login',
        element: <JwtLoginPage />,
      },
    ],
  },
];
