import { Suspense, lazy } from "react";
import { Outlet } from "react-router";
import { paths } from "../paths";
import { SplashScreen } from "../../components/loading-screen";
import { GuestGuard } from "../../components/guard";

const JwtLoginPage = lazy(
  () => import("../../pages/auth/views/jwt-login-view")
);

export const authRoutes = [
  {
    element: (
      <Suspense fallback={<SplashScreen />}>
        <Outlet />
      </Suspense>
    ),
    children: [
      {
        path: "login",
        element: (
          <GuestGuard>
            <JwtLoginPage />
          </GuestGuard>
        ),
      },
    ],
  },
];
