import { describe, test, expect, jest, beforeEach } from '@jest/globals';
import { configureStore } from '@reduxjs/toolkit';
import authReducer, {
  clearAuthState,
  clearError,
  initialize,
  login,
  logout,
  register,
} from './authSlice';
import { IUserProfile } from '@/types/profile';
import * as authUtils from '../auth';
import axiosInstance, { APIResponse } from '@/utils/axios';
import * as accountApi from '@/apis/account';
import {
  ILoginRequest,
  ILoginResponse,
  IRegisterRequest,
  IRegisterResponse,
} from '@/types/account';
import { AppDispatch } from '../store';

// Định nghĩa kiểu cho state của auth slice
interface AuthState {
  user: IUserProfile | null;
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
}

// Định nghĩa RootState cho store
interface RootState {
  auth: AuthState;
}

jest.mock('@/apis/account');
jest.mock('@/utils/axios');
jest.mock('../auth');

describe('authSlice', () => {
  let store: ReturnType<typeof configureStore<RootState>>;
  let dispatch: AppDispatch;

  const initialState: AuthState = {
    user: null,
    token: null,
    isAuthenticated: false,
    loading: false,
    error: null,
  };

  const mockUser: IUserProfile = {
    userID: 1,
    firstName: 'test',
    lastName: 'user',
    fullname: 'test user',
    avatar: 'test avatar',
    dateOfBirth: new Date(),
    phone: '123456789',
    email: 'test@example.com',
    gender: 1,
    bio: 'test bio',
  };

  const mockToken = 'mockAccessToken';
  const mockRefreshToken = 'mockRefreshToken';

  beforeEach(() => {
    // Mock axiosInstance với Authorization
    jest.mock('@/utils/axios', () => {
      const axiosInstance = {
        defaults: {
          headers: {
            common: {
              Authorization: undefined, // Khởi tạo Authorization
            },
          },
        },
      };

      return { axiosInstance };
    });

    store = configureStore<RootState>({ reducer: { auth: authReducer } });
    dispatch = store.dispatch;
    jest.clearAllMocks();

    // Mock localStorage
    jest.spyOn(Storage.prototype, 'getItem').mockReturnValue(null);
    jest.spyOn(Storage.prototype, 'setItem').mockImplementation(() => {});
    jest.spyOn(Storage.prototype, 'removeItem').mockImplementation(() => {});

    // Mock authUtils
    jest.spyOn(authUtils, 'setSession').mockImplementation(() => {});
    jest.spyOn(authUtils, 'setRefreshToken').mockImplementation(() => {});
    jest.spyOn(authUtils, 'isValidToken').mockReturnValue(false);
    jest.spyOn(authUtils, 'jwtDecode').mockReturnValue({ Data: null });
  });

  // --- Reducer actions ---
  describe('reducer actions', () => {
    test('clearAuthState resets state to initial', () => {
      dispatch(clearAuthState());
      const newState = store.getState().auth;
      expect(newState).toEqual(initialState);
    });

    test('clearError clears error state', () => {
      dispatch(clearError());
      const newState = store.getState().auth;
      expect(newState).toEqual(initialState);
    });

    test('initialize with valid token', () => {
      jest.spyOn(Storage.prototype, 'getItem').mockReturnValue(mockToken);
      jest.spyOn(authUtils, 'isValidToken').mockReturnValue(true);
      jest.spyOn(authUtils, 'jwtDecode').mockReturnValue({ Data: mockUser });

      dispatch(initialize());

      expect(authUtils.setSession).toHaveBeenCalledWith(mockToken);
      const newState = store.getState().auth;

      const expectedState = {
        ...initialState,
        user: mockUser,
        token: mockToken,
        isAuthenticated: true,
      };

      expect(newState).toEqual(expectedState);
    });

    test('initialize with invalid token', () => {
      jest.spyOn(Storage.prototype, 'getItem').mockReturnValue(mockToken);
      jest.spyOn(authUtils, 'isValidToken').mockReturnValue(false);

      dispatch(initialize());

      expect(authUtils.setSession).not.toHaveBeenCalled();
      const newState = store.getState().auth;
      expect(newState).toEqual(initialState);
    });

    test('logout clears state and removes token', () => {
      jest.spyOn(authUtils, 'setSession').mockImplementation(() => {});
      jest.spyOn(authUtils, 'setRefreshToken').mockImplementation(() => {});
      dispatch(logout());

      expect(axiosInstance.defaults.headers.common.Authorization).toBeUndefined();
      expect(authUtils.setSession).toHaveBeenCalledWith(null);
      expect(authUtils.setRefreshToken).toHaveBeenCalledWith(null);
      const newState = store.getState().auth;
      expect(newState).toEqual(initialState);
    });
  });

  // --- Async thunks ---
  describe('login', () => {
    const loginRequest: ILoginRequest = { username: 'testuser', password: 'password123' };

    test('login pending', () => {
      jest.spyOn(accountApi, 'loginApi').mockReturnValue(new Promise(() => {}));
      dispatch(login(loginRequest));

      const state = store.getState().auth;
      expect(state.loading).toBe(true);
      expect(state.error).toBeNull();
    });

    test('login fulfilled', async () => {
      const mockResponse: APIResponse<ILoginResponse> = {
        code: 1,
        message: 'Login successful',
        data: { accessToken: mockToken, refreshToken: mockRefreshToken, userProfile: mockUser },
      };
      jest.spyOn(accountApi, 'loginApi').mockResolvedValue(mockResponse);

      // Act
      await dispatch(login(loginRequest));

      // Assert
      const newState = store.getState().auth;

      const expectedState = {
        ...initialState,
        user: mockUser,
        token: mockToken,
        isAuthenticated: true,
        loading: false,
      };
      expect(accountApi.loginApi).toHaveBeenCalledWith(loginRequest);
      expect(authUtils.setSession).toHaveBeenCalledWith(mockToken);
      expect(authUtils.setRefreshToken).toHaveBeenCalledWith(mockRefreshToken);
      expect(newState).toEqual(expectedState);
    });

    test('login rejected', async () => {
      const error = { message: 'Login failed' };
      jest.spyOn(accountApi, 'loginApi').mockRejectedValue(error);

      // Act
      await dispatch(login(loginRequest));

      // Assert
      const newState = store.getState().auth;

      const expectedState = {
        ...initialState,
        error: error.message,
        loading: false,
      };
      expect(accountApi.loginApi).toHaveBeenCalledWith(loginRequest);
      expect(newState).toEqual(expectedState);
    });
  });

  describe('register', () => {
    const registerRequest: IRegisterRequest = {
      username: 'testuser',
      firstName: 'test',
      lastName: 'user',
      password: 'password123',
    };

    test('register pending', () => {
      jest.spyOn(accountApi, 'registerApi').mockReturnValue(new Promise(() => {}));

      // Act
      dispatch(register(registerRequest));

      // Assert
      const state = store.getState().auth;
      expect(state.loading).toBe(true);
      expect(state.error).toBeNull();
    });

    test('register fulfilled', async () => {
      const mockResponse: APIResponse<IRegisterResponse> = {
        code: 1,
        message: 'Register successful',
        data: { accessToken: mockToken, refreshToken: mockRefreshToken, userProfile: mockUser },
      };
      jest.spyOn(accountApi, 'registerApi').mockResolvedValue(mockResponse);

      // Act
      await dispatch(register(registerRequest));

      // Assert
      const newState = store.getState().auth;

      const expectedState = {
        ...initialState,
        user: mockUser,
        token: mockToken,
        isAuthenticated: true,
        loading: false,
      };
      expect(accountApi.registerApi).toHaveBeenCalledWith(registerRequest);
      expect(authUtils.setSession).toHaveBeenCalledWith(mockToken);
      expect(authUtils.setRefreshToken).toHaveBeenCalledWith(mockRefreshToken);
      expect(newState).toEqual(expectedState);
    });

    test('register rejected', async () => {
      const error = { message: 'Register failed' };
      jest.spyOn(accountApi, 'registerApi').mockRejectedValue(error);

      // Act
      await dispatch(register(registerRequest));

      // Assert
      const newState = store.getState().auth;

      const expectedState = {
        ...initialState,
        error: error.message,
        loading: false,
      };
      expect(accountApi.registerApi).toHaveBeenCalledWith(registerRequest);
      expect(newState).toEqual(expectedState);
    });
  });
});
