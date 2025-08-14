import { useState, useEffect, useCallback } from "react";
import { paths } from "../../routes/paths";
import { useAuthContext } from "../../stores/auth";
import { useRouter } from "../../routes/hooks";
import { LoadingScreen } from "../loading-screen";

// ----------------------------------------------------------------------

const loginPaths: Record<string, string> = {
  jwt: paths.auth.login,
};

// const publicPaths = [
//   paths.auth.jwt.login,
//   paths.auth.jwt.register,
//   paths.auth.jwt.forgotPassword,
//   // Add other public paths as needed
// ];

// ----------------------------------------------------------------------

type Props = {
  children: React.ReactNode;
};

export default function AuthGuard({ children }: Props) {
  const { loading } = useAuthContext();

  return <>{loading ? <LoadingScreen /> : <Container>{children}</Container>}</>;
}

// ----------------------------------------------------------------------

function Container({ children }: Props) {
  const router = useRouter();

  const { authenticated, method } = useAuthContext();

  const [checked, setChecked] = useState(false);

  const check = useCallback(() => {
    // const isPublicPath = publicPaths.some((path) => window.location.pathname.includes(path));
    if (!authenticated) {
      const searchParams = new URLSearchParams({
        returnTo: window.location.pathname,
      }).toString();

      const loginPath = loginPaths[method];

      const href = `${loginPath}?${searchParams}`;

      router.replace(href);
    } else {
      setChecked(true);
    }
  }, [authenticated, method, router]);

  useEffect(() => {
    check();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (!checked) {
    return null;
  }

  return <>{children}</>;
}
