import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { ILoginRequest, IRegisterRequest } from '@/types/account';
import { loginApi, registerApi } from '@/apis/account';
import { IUserProfile } from '@/types/profile';
// import axiosInstance from '@/utils/axios';
import { isValidToken, jwtDecode, setRefreshToken, setSession } from '../auth';

type LoginPayload = { user: IUserProfile; token: string };
type RegisterPayload = LoginPayload;

export type AuthStateType = {
  user: IUserProfile | null;
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
};

const STORAGE_KEY = 'accessToken';

// Async thunks
export const login = createAsyncThunk<
  LoginPayload, // return type
  ILoginRequest, // arg type
  { rejectValue: string } // reject value type
>('auth/login', async (request, thunkAPI) => {
  try {
    const { data } = await loginApi(request);
    setSession(data.accessToken);
    setRefreshToken(data.refreshToken);

    return { user: data.userProfile, token: data.accessToken };
  } catch (error) {
    return thunkAPI.rejectWithValue(error instanceof Error ? error.message : 'Login failed');
  }
});

export const register = createAsyncThunk<
  RegisterPayload,
  IRegisterRequest,
  { rejectValue: string }
>('auth/register', async (request, thunkAPI) => {
  try {
    const { data } = await registerApi(request);
    setSession(data.accessToken);
    setRefreshToken(data.refreshToken);
    return { user: data.userProfile, token: data.accessToken };
  } catch (error) {
    return thunkAPI.rejectWithValue(error instanceof Error ? error.message : 'Register failed');
  }
});

// Initial state
const initialState: AuthStateType = {
  user: null,
  token: null,
  isAuthenticated: false,
  loading: false,
  error: null,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    clearAuthState: (state) => {
      Object.assign(state, initialState);
    },
    clearError: (state) => {
      state.error = null;
    },
    initialize: (state) => {
      const accessToken = localStorage.getItem(STORAGE_KEY);

      if (accessToken && isValidToken(accessToken)) {
        const tokenDecode = jwtDecode(accessToken);
        setSession(accessToken);
        state.user = tokenDecode.Data;
        state.token = accessToken;
        state.isAuthenticated = true;
      }
    },
    logout: (state) => {
      setSession(null);
      setRefreshToken(null);
      Object.assign(state, initialState);
    },
  },
  extraReducers: (builder) => {
    // Login
    builder.addCase(login.pending, (state) => {
      state.loading = true;
      state.error = null;
    });
    builder.addCase(login.fulfilled, (state, action) => {
      state.user = action.payload.user;
      state.token = action.payload.token;
      state.isAuthenticated = true;
      state.loading = false;
    });
    builder.addCase(login.rejected, (state, action) => {
      state.error = action.payload ?? 'Something went wrong';
      state.loading = false;
    });

    // Register
    builder.addCase(register.pending, (state) => {
      state.loading = true;
      state.error = null;
    });
    builder.addCase(register.fulfilled, (state, action) => {
      state.user = action.payload.user;
      state.token = action.payload.token;
      state.isAuthenticated = true;
      state.loading = false;
    });
    builder.addCase(register.rejected, (state, action) => {
      state.error = action.payload ?? 'Something went wrong';
      state.loading = false;
    });
  },
});

export const { clearAuthState, clearError, initialize, logout } = authSlice.actions;

export default authSlice.reducer;
