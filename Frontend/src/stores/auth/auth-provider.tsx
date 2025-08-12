import {
  useReducer,
  createContext,
  useCallback,
  useEffect,
  useMemo,
} from "react";
import { IUserProfile } from "../../types/profile";
import { AuthStateType, JWTContextType } from "./type";
import { isValidToken, jwtDecode, setRefreshToken, setSession } from "./utils";
import { ILoginRequest, IRegisterRequest } from "../../types/account";
import { loginApi, registerApi } from "../../apis/account";
import axios from "axios";

enum Types {
  INITIAL = "INITIAL",
  LOGIN = "LOGIN",
  REGISTER = "REGISTER",
  LOGOUT = "LOGOUT",
}

type Payload = {
  [Types.INITIAL]: {
    user: IUserProfile | null;
  };
  [Types.LOGIN]: {
    user: IUserProfile | null;
  };
  [Types.REGISTER]: {
    user: IUserProfile | null;
  };
  [Types.LOGOUT]: undefined;
};

type AuthAction =
  | {
      type: Types.INITIAL;
      payload: Payload[Types.INITIAL];
    }
  | {
      type: Types.LOGIN;
      payload: Payload[Types.LOGIN];
    }
  | {
      type: Types.REGISTER;
      payload: Payload[Types.REGISTER];
    }
  | {
      type: Types.LOGOUT;
      payload: Payload[Types.LOGOUT];
    };

const initialState: AuthStateType = {
  user: null,
  loading: true,
};

const reducer = (state: AuthStateType, action: AuthAction) => {
  switch (action.type) {
    case Types.INITIAL:
      return { ...state, loading: false };
    case Types.LOGIN:
      return { ...state, user: action.payload.user, loading: false };
    case Types.REGISTER:
      return { ...state, user: action.payload.user, loading: false };
    case Types.LOGOUT:
      return { ...state, user: null, loading: false };
    default:
      return state;
  }
};

export const AuthContext = createContext<JWTContextType | undefined>(undefined);

const STORAGE_KEY = "accessToken";

const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [state, dispatch] = useReducer(reducer, initialState);

  const toCamelCase = useCallback((o: any): any => {
    if (Array.isArray(o)) {
      return o.map((value: any) => {
        if (typeof value === "object" && value !== null) {
          return toCamelCase(value);
        }
        return value;
      });
    }

    const newO: any = {};
    Object.keys(o).forEach((origKey) => {
      if (Object.prototype.hasOwnProperty.call(o, origKey)) {
        const newKey = (
          origKey.charAt(0).toLowerCase() + origKey.slice(1)
        ).toString();

        let value = o[origKey];

        if (
          Array.isArray(value) ||
          (value !== null && typeof value === "object")
        ) {
          value = toCamelCase(value);
        }

        newO[newKey] = value;
      }
    });

    return newO;
  }, []);

  const parseWithStringValues = useCallback(
    (jsonString: string) => {
      try {
        const parsed = JSON.parse(jsonString);
        return toCamelCase(parsed);
      } catch (error) {
        console.error("Error parsing JSON:", error);
        throw error;
      }
    },
    [toCamelCase]
  );

  const initialize = useCallback(async () => {
    try {
      const accessToken = localStorage.getItem(STORAGE_KEY);

      if (accessToken && isValidToken(accessToken)) {
        
        const tokenDecode = jwtDecode(accessToken);

        const profile = parseWithStringValues(tokenDecode.Data as string);

        setSession(accessToken);

        dispatch({
          type: Types.INITIAL,
          payload: {
            user: {
              ...profile,
              accessToken,
            },
          },
        });
      } else {
        dispatch({
          type: Types.INITIAL,
          payload: {
            user: null,
          },
        });
      }
    } catch (error) {
      console.error(error);
      dispatch({
        type: Types.INITIAL,
        payload: {
          user: null,
        },
      });
    }
  }, [parseWithStringValues]);

  useEffect(() => {
    initialize();
  }, [initialize]);

  const login = useCallback(async (payload: ILoginRequest) => {
    try {
      const { data } = await loginApi(payload);

      const { accessToken, refreshToken, profile } = data;

      setSession(accessToken);
      setRefreshToken(refreshToken);

      dispatch({
        type: Types.LOGIN,
        payload: {
          user: {
            ...profile,
          },
        },
      });
    } catch (error) {
      console.error(error);
    }
  }, []);

  const register = useCallback(async (payload: IRegisterRequest) => {
    try {
      const { data } = await registerApi(payload);

      const { accessToken, refreshToken, profile } = data;

      setSession(accessToken);
      setRefreshToken(refreshToken);

      dispatch({
        type: Types.LOGIN,
        payload: {
          user: {
            ...profile,
          },
        },
      });
    } catch (error) {
      console.error(error);
    }
  }, []);

  const logout = useCallback(async () => {
    try {
      localStorage.removeItem(STORAGE_KEY);
      delete axios.defaults.headers.common.Authorization;
      dispatch({
        type: Types.LOGOUT,
        payload: undefined,
      });
    } catch (error) {
      console.error(error);
    }
  }, []);

  // ----------------------------------------------------------------------

  const checkAuthenticated = state.user ? "authenticated" : "unauthenticated";

  const status = state.loading ? "loading" : checkAuthenticated;

  const memoizedValue = useMemo(
    () => ({
      user: state.user,
      method: "jwt",
      loading: status === "loading",
      authenticated: status === "authenticated",
      unauthenticated: status === "unauthenticated",
      //
      login,
      register,
      logout,
    }),
    [login, logout, register, state.user, status]
  );

  return (
    <AuthContext.Provider value={memoizedValue}>
      {children}
    </AuthContext.Provider>
  );
};


export default AuthProvider;