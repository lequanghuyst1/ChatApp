import { useEffect, useCallback } from 'react';
import { paths } from '@/routes/paths';
import { useRouter, useSearchParams } from '@/routes/hooks';
import { useAppSelector } from '@/stores/hook';
import { LoadingScreen } from '../loading-screen';

// ----------------------------------------------------------------------

type Props = {
  children: React.ReactNode;
};

export default function GuestGuard({ children }: Props) {
  const { loading } = useAppSelector((state) => state.auth);

  return <>{loading ? <LoadingScreen /> : <Container>{children}</Container>}</>;
}

// ----------------------------------------------------------------------

type ContainerProps = {
  children: React.ReactNode;
};

function Container({ children }: ContainerProps) {
  const { isAuthenticated } = useAppSelector((state) => state.auth);
  const router = useRouter();

  const searchParams = useSearchParams();

  const returnTo = searchParams.get('returnTo') || paths.dashboard.root;

  const check = useCallback(() => {
    if (isAuthenticated) {
      router.replace(returnTo);
    }
  }, [isAuthenticated, returnTo, router]);

  useEffect(() => {
    check();
  }, [check]);

  return <>{children}</>;
}
