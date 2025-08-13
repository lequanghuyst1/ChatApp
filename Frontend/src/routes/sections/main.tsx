import { Suspense, lazy } from "react";
import { Outlet } from "react-router";
import { paths } from "../paths";
import { SplashScreen } from "../../components/loading-screen";
import { AuthGuard } from "../../components/guard";

const JwtLoginPage = lazy(
  () => import("../../pages/auth/views/jwt-login-view")
);

export const mainRoutes = [
  {
    element: (
      <AuthGuard>
        <Suspense fallback={<SplashScreen />}>
          <Outlet />
        </Suspense>
      </AuthGuard>
    ),
    children: [
      {
        path: paths.dashboard.root,
        element: <JwtLoginPage />,
      },
    ],
  },
];
