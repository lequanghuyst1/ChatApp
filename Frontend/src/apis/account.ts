import {
  ILoginRequest,
  ILoginResponse,
  IRegisterRequest,
  IRegisterResponse,
} from '@/types/account';
import axiosInstance, { APIResponse, endpoints } from '@/utils/axios';

const URL = endpoints.auth;

export const loginApi = async (payload: ILoginRequest): Promise<APIResponse<ILoginResponse>> => {
  const response = await axiosInstance.post<APIResponse<ILoginResponse>>(`${URL.login}`, payload);

  const { code, data, message } = response.data;

  if (code !== 1) {
    throw new Error(message);
  }

  return { code, data, message };
};

export const registerApi = async (
  payload: IRegisterRequest
): Promise<APIResponse<IRegisterResponse>> => {
  const response = await axiosInstance.post<APIResponse<IRegisterResponse>>(
    `${URL.register}`,
    payload
  );

  const { code, data, message } = response.data;

  if (code !== 1) {
    throw new Error(message);
  }

  return { code, data, message };
};
