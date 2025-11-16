import { ILoginRequest, IRegisterRequest } from '@/types/account';
import { IUserProfile } from '@/types/profile';

export type JWTContextType = {
  user: IUserProfile | null;
  method: string;
  loading: boolean;
  authenticated: boolean;
  unauthenticated: boolean;
  login: (payload: ILoginRequest) => Promise<void>;
  register: (payload: IRegisterRequest) => Promise<void>;
  logout: () => Promise<void>;
};

export type AuthStateType = {
  status?: string;
  loading: boolean;
  user: IUserProfile | null;
};
