// ----------------------------------------------------------------------

import { paths } from '@/routes/paths';
import axiosInstance, { endpoints } from '@/utils/axios';

export function jwtDecode(token: string) {
  const base64Url = token.split('.')[1];
  const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
  const jsonPayload = decodeURIComponent(
    window
      .atob(base64)
      .split('')
      .map((c) => `%${`00${c.charCodeAt(0).toString(16)}`.slice(-2)}`)
      .join('')
  );

  return JSON.parse(jsonPayload);
}

// ----------------------------------------------------------------------

export const isValidToken = (accessToken: string) => {
  if (!accessToken) {
    return false;
  }

  const decoded = jwtDecode(accessToken);

  const currentTime = Date.now() / 1000;

  return decoded.exp > currentTime;
};

// ----------------------------------------------------------------------

// export const tokenExpired = (exp: number) => {
//   // eslint-disable-next-line prefer-const
//   let expiredTimer;

//   const currentTime = Date.now();

//   // Test token expires after 10s
//   // const timeLeft = currentTime + 10000 - currentTime; // ~10s
//   const timeLeft = exp * 1000 - currentTime;

//   clearTimeout(expiredTimer);

//   expiredTimer = setTimeout(() => {
//     // alert('Token expired');
//     // localStorage.removeItem('accessToken');
//     // window.location.href = paths.auth.jwt.login;
//     reFreshToken();
//   }, timeLeft);
// };

let visibilityListenerAttached = false;

export const tokenExpired = (exp: number) => {
  const refreshBeforeMs = 30 * 1000; // 30 seconds

  const checkExpiration = () => {
    const currentTime = Date.now();
    const expTime = exp * 1000;
    const timeLeft = expTime - currentTime;

    if (timeLeft <= refreshBeforeMs) {
      // console.log('Token near expiration, refreshing...');
      reFreshToken();
    } else {
      const nextCheck = timeLeft - refreshBeforeMs;
      // console.log(`Token valid, scheduling refresh in ${nextCheck}ms`);
      setTimeout(checkExpiration, nextCheck);
    }
  };

  checkExpiration();

  if (!visibilityListenerAttached) {
    window.addEventListener('visibilitychange', () => {
      if (document.visibilityState === 'visible') {
        checkExpiration();
      }
    });
    visibilityListenerAttached = true;
  }
};

// ----------------------------------------------------------------------

export const setSession = (accessToken: string | null) => {
  if (accessToken) {
    sessionStorage.setItem('accessToken', accessToken);
    localStorage.setItem('accessToken', accessToken);

    axiosInstance.defaults.headers.common.Authorization = `Bearer ${accessToken}`;

    // This function below will handle when token is expired
    const { exp } = jwtDecode(accessToken); // ~3 days by minimals server
    tokenExpired(exp);
  } else {
    localStorage.removeItem('accessToken');

    delete axiosInstance.defaults.headers.common.Authorization;
  }
};

// ----------------------------------------------------------------------

export const setRefreshToken = (refreshToken: string | null) => {
  if (refreshToken) {
    localStorage.setItem('refreshToken', refreshToken);

    // This function below will handle when token is expired
    // const { exp } = jwtDecode(refreshToken); // ~3 days by minimals server
    // tokenExpired(exp);
  } else {
    localStorage.removeItem('refreshToken');

    delete axiosInstance.defaults.headers.common.Authorization;
  }
};

// ----------------------------------------------------------------------

export const reFreshToken = async () => {
  const refreshToken = localStorage.getItem('refreshToken');
  if (!refreshToken) {
    window.location.href = paths.auth.login;
    return;
  }

  try {
    const response = await axiosInstance.post(endpoints.auth.retoken, { refreshToken });

    const { accessToken, refreshToken: newRefreshToken } = response.data.data;

    setSession(accessToken);
    setRefreshToken(newRefreshToken);
  } catch {
    // console.error('Error refreshing token');
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    window.location.href = paths.auth.login;
  }
};
