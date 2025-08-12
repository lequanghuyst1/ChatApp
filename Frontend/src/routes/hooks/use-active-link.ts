import { matchPath, useLocation } from 'react-router-dom';

// ----------------------------------------------------------------------

type ReturnType = boolean;

export function useActiveLink(path: string, deep = true): ReturnType {
  const { pathname } = useLocation();

  const normalActive = path ? !!matchPath({ path, end: true }, pathname) : false;

  const deepActive = path ? !!matchPath({ path, end: false }, pathname) : false;

  const hierarchyActive = (path.split('/').length > 2 && pathname.split('/').length > 3 && pathname.startsWith(path));

  return deep ? deepActive : (normalActive || hierarchyActive);
}
