import {
  ILoginRequest,
  ILoginResponse,
  IRegisterRequest,
  IRegisterResponse,
} from '@/types/account';
import { APIResponse } from '@/utils/axios';
import * as accountApi from '@/apis/account';
import { IUserProfile } from '@/types/profile';

describe('account', () => {
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

  beforeEach(() => {
    jest.clearAllMocks();
  });

  describe('login', () => {
    const loginRequest: ILoginRequest = {
      username: 'testuser',
      password: 'password123',
    };

    test('login success', async () => {
      const mockResponse: APIResponse<ILoginResponse> = {
        code: 1,
        message: 'Login successful',
        data: { accessToken: 'mockToken', refreshToken: 'mockRefreshToken', userProfile: mockUser },
      };

      jest.spyOn(accountApi, 'loginApi').mockResolvedValue(mockResponse);

      const result = await accountApi.loginApi(loginRequest);

      expect(result).toEqual(mockResponse);
    });

    test('login rejected', async () => {
      const errorMessage = 'Login failed';
      const error = new Error(errorMessage);

      jest.spyOn(accountApi, 'loginApi').mockRejectedValue(error);

      await expect(accountApi.loginApi(loginRequest)).rejects.toThrow(errorMessage);
    });
  });

  describe('register', () => {
    const registerRequest: IRegisterRequest = {
      username: 'testuser',
      firstName: 'test',
      lastName: 'user',
      password: 'password123',
    };

    test('register success', async () => {
      const mockResponse: APIResponse<IRegisterResponse> = {
        code: 1,
        message: 'Register successful',
        data: { accessToken: 'mockToken', refreshToken: 'mockRefreshToken', userProfile: mockUser },
      };

      jest.spyOn(accountApi, 'registerApi').mockResolvedValue(mockResponse);

      const result = await accountApi.registerApi(registerRequest);

      expect(result).toEqual(mockResponse);
    });

    test('register rejected', async () => {
      const errorMessage = 'Register failed';
      const error = new Error(errorMessage);

      jest.spyOn(accountApi, 'registerApi').mockRejectedValue(error);

      await expect(accountApi.registerApi(registerRequest)).rejects.toThrow(errorMessage);
    });
  });
});
