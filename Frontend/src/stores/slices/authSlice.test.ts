import { describe, test, expect, jest, beforeEach } from '@jest/globals';
import { configureStore, ThunkDispatch } from '@reduxjs/toolkit';
import { AnyAction } from 'redux';
import authReducer, { clearAuthState, clearError, initialize, logout, login } from './authSlice';
import { ILoginRequest, ILoginResponse, IRegisterRequest } from '@/types/account';
import { IUserProfile } from '@/types/profile';
import * as authUtils from '../auth';
import * as accountApi from '@/apis/account';
import axiosInstance, { APIResponse } from '@/utils/axios';

// Mock dependencies
jest.mock('@/apis/account');
jest.mock('@/utils/axios');
jest.mock('../auth');

describe('authSlice', () => {
  let store: ReturnType<typeof configureStore> & {
    dispatch: ThunkDispatch<unknown, unknown, AnyAction>;
  };

  const initialState = {
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
    store = configureStore({
      reducer: {
        auth: authReducer,
      },
    }) as typeof store;
    jest.clearAllMocks();
  });

  // Test reducer actions
  describe('reducer actions', () => {
    test('clearAuthState resets state to initial', () => {
      const modifiedState = {
        ...initialState,
        user: mockUser,
        token: mockToken,
        isAuthenticated: true,
        error: 'some error',
      };
      const newState = authReducer(modifiedState, clearAuthState());
      expect(newState).toEqual(initialState);
    });

    test('clearError clears error state', () => {
      const stateWithError = { ...initialState, error: 'some error' };
      const newState = authReducer(stateWithError, clearError());
      expect(newState.error).toBeNull();
      expect(newState).toEqual({ ...stateWithError, error: null });
    });

    test('initialize sets state when valid token exists', () => {
      const mockDecodedToken = { Data: mockUser };
      (authUtils.isValidToken as jest.Mock).mockReturnValue(true);
      (authUtils.jwtDecode as jest.Mock).mockReturnValue(mockDecodedToken);
      localStorage.setItem('accessToken', mockToken);

      const newState = authReducer(initialState, initialize());
      expect(authUtils.setSession).toHaveBeenCalledWith(mockToken);
      expect(newState).toEqual({
        ...initialState,
        user: mockUser,
        token: mockToken,
        isAuthenticated: true,
      });
    });

    test('initialize does nothing with invalid token', () => {
      (authUtils.isValidToken as jest.Mock).mockReturnValue(false);
      localStorage.setItem('accessToken', mockToken);

      const newState = authReducer(initialState, initialize());
      expect(newState).toEqual(initialState);
    });

    test('logout clears state and removes token', () => {
      const modifiedState = {
        ...initialState,
        user: mockUser,
        token: mockToken,
        isAuthenticated: true,
      };
      const newState = authReducer(modifiedState, logout());
      expect(localStorage.removeItem).toHaveBeenCalledWith('accessToken');
      expect(axiosInstance.defaults.headers.common.Authorization).toBeUndefined();
      expect(newState).toEqual(initialState);
    });
  });

  // Test async thunks
//   describe('async thunks', () => {
//     describe('login', () => {
//       const loginRequest: ILoginRequest = {
//         username: 'testuser',
//         password: 'password123',
//       };

//       test('login fulfilled', async () => {
//         const mockResponse: APIResponse<ILoginResponse> = {
//           code: 1,
//           message: 'Login successful',
//           data: {
//             accessToken: mockToken,
//             refreshToken: mockRefreshToken,
//             profile: mockUser,
//           },
//         };
//         (accountApi.loginApi as jest.Mock).mockResolvedValue(mockResponse);

//         await store.dispatch(login(loginRequest));
//         const state = store.getState().auth;

//         expect(accountApi.loginApi).toHaveBeenCalledWith(loginRequest);
//         expect(authUtils.setSession).toHaveBeenCalledWith(mockToken);
//         expect(authUtils.setRefreshToken).toHaveBeenCalledWith(mockRefreshToken);
//         expect(state).toEqual({
//           ...initialState,
//           user: mockUser,
//           token: mockToken,
//           isAuthenticated: true,
//         });
//       });

//       test('login rejected', async () => {
//         const errorMessage = 'Invalid credentials';
//         (accountApi.loginApi as jest.Mock).mockRejectedValue(new Error(errorMessage));

//         await store.dispatch(login(loginRequest));
//         const state = store.getState().auth;

//         expect(state).toEqual({
//           ...initialState,
//           error: errorMessage,
//           loading: false,
//         });
//       });

//       test('login pending', async () => {
//         const mockResponse = {
//           data: {
//             accessToken: mockToken,
//             refreshToken: mockRefreshToken,
//             profile: mockUser,
//           },
//         };
//         (accountApi.loginApi as jest.Mock).mockReturnValue(new Promise(() => {}));

//         const promise = store.dispatch(login(loginRequest));
//         const state = store.getState().auth;

//         expect(state.loading).toBe(true);
//         expect(state.error).toBeNull();

//         // Clean up pending promise
//         (accountApi.loginApi as jest.Mock).mockResolvedValue(mockResponse);
//         await promise;
//       });
//     });

//     describe('register', () => {
//       const registerRequest: IRegisterRequest = {
//         username: 'testuser',
//         email: 'test@example.com',
//         password: 'password123',
//       };

//       test('register fulfilled', async () => {
//         const mockResponse = {
//           data: {
//             accessToken: mockToken,
//             refreshToken: mockRefreshToken,
//             profile: mockUser,
//           },
//         };
//         (accountApi.registerApi as jest.Mock).mockResolvedValue(mockResponse);

//         await store.dispatch(register(registerRequest));
//         const state = store.getState().auth;

//         expect(accountApi.registerApi).toHaveBeenCalledWith(registerRequest);
//         expect(authUtils.setSession).toHaveBeenCalledWith(mockToken);
//         expect(authUtils.setRefreshToken).toHaveBeenCalledWith(mockRefreshToken);
//         expect(state).toEqual({
//           ...initialState,
//           user: mockUser,
//           token: mockToken,
//           isAuthenticated: true,
//         });
//       });

//       test('register rejected', async () => {
//         const errorMessage = 'Registration failed';
//         (accountApi.registerApi as jest.Mock).mockRejectedValue(new Error(errorMessage));

//         await store.dispatch(register(registerRequest));
//         const state = store.getState().auth;

//         expect(state).toEqual({
//           ...initialState,
//           error: errorMessage,
//           loading: false,
//         });
//       });

//       test('register pending', async () => {
//         const mockResponse = {
//           data: {
//             accessToken: mockToken,
//             refreshToken: mockRefreshToken,
//             profile: mockUser,
//           },
//         };
//         (accountApi.registerApi as jest.Mock).mockReturnValue(new Promise(() => {}));

//         const promise = store.dispatch(register(registerRequest));
//         const state = store.getState().auth;

//         expect(state.loading).toBe(true);
//         expect(state.error).toBeNull();

//         // Clean up pending promise
//         (accountApi.registerApi as jest.Mock).mockResolvedValue(mockResponse);
//         await promise;
//       });
//     });
//   });
});