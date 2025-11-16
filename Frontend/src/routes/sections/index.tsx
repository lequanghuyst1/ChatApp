import { Navigate, useRoutes } from 'react-router-dom';
import { mainRoutes } from './main';
import { authRoutes } from './auth';

// ----------------------------------------------------------------------

export default function Router() {
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
    { path: '*', element: <Navigate to="/login" replace /> },
  ]);
}
