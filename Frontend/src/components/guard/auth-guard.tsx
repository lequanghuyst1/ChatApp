import { useState, useEffect, useCallback } from 'react';
import { useAppSelector } from '@/stores/hook';
import { paths } from '@/routes/paths';
import { useRouter } from '@/routes/hooks';
import { LoadingScreen } from '../loading-screen';

// ----------------------------------------------------------------------

const loginPaths: Record<string, string> = {
  jwt: paths.auth.login,
};

type Props = {
  children: React.ReactNode;
};

export default function AuthGuard({ children }: Props) {
  const { isAuthenticated, loading } = useAppSelector((state) => state.auth);

  return (
    <>
      {loading ? (
        <LoadingScreen />
      ) : (
        <Container isAuthenticated={isAuthenticated}>{children}</Container>
      )}
    </>
  );
}

// ----------------------------------------------------------------------

type ContainerProps = {
  children: React.ReactNode;
  isAuthenticated: boolean;
};

function Container({ children, isAuthenticated }: ContainerProps) {
  const router = useRouter();

  const [checked, setChecked] = useState(false);

  const check = useCallback(() => {
    if (!isAuthenticated) {
      const searchParams = new URLSearchParams({
        returnTo: window.location.pathname,
      }).toString();

      const loginPath = loginPaths['jwt'];

      const href = `${loginPath}?${searchParams}`;

      router.replace(href);
    } else {
      setChecked(true);
    }
  }, [isAuthenticated, router]);

  useEffect(() => {
    check();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (!checked) {
    return null;
  }

  return <>{children}</>;
}
