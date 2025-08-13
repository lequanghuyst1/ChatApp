import { Navigate, useRoutes } from "react-router-dom";
import { useAuthContext } from "../../stores/auth";
import { mainRoutes } from "./main";
import { authRoutes } from "./auth";

// ----------------------------------------------------------------------

export default function Router() {
  const { user } = useAuthContext();

  return useRoutes([
    // {
    //   path: "/",
    //   element: <> </>,
    // },

    // Main routes
    ...mainRoutes,

    // Auth routes
    ...authRoutes,

    // No match 404
    { path: "*", element: <Navigate to="/login" replace /> },
  ]);
}
