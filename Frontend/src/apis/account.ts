import {
  ILoginRequest,
  ILoginResponse,
  IRegisterRequest,
  IRegisterResponse,
} from "../types/account";
import axiosInstance, { APIResponse, endpoints } from "../utils/axios";

const URL = endpoints.auth;

export const loginApi = async (
  payload: ILoginRequest
): Promise<APIResponse<ILoginResponse>> => {
  try {
    const response = await axiosInstance.post<APIResponse<ILoginResponse>>(
      `${URL.login}`,
      payload
    );
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const registerApi = async (
  payload: IRegisterRequest
): Promise<APIResponse<IRegisterResponse>> => {
  try {
    const response = await axiosInstance.post<APIResponse<IRegisterResponse>>(
      `${URL.register}`,
      payload
    );
    return response.data;
  } catch (error) {
    throw error;
  }
};
