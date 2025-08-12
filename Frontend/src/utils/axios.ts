import axios, { AxiosRequestConfig } from "axios";
import { HOST_CHAT_API } from "../config-global";

export interface APIResponse<T = any> {
  data: T;
  code: number;
  message: string;
}

const axiosInstance = axios.create({
  baseURL: HOST_CHAT_API,
  headers: {
    "Content-Type": "application/json",
  },
});

axiosInstance.interceptors.response.use(
  (response) => response,
  (error) =>
    Promise.reject(
      (error.response && error.response.data) ||
        error.message ||
        "Something went wrong"
    )
);

export default axiosInstance;

export const fetcher = async (args: string | [string, AxiosRequestConfig]) => {
  const [url, config] = Array.isArray(args) ? args : [args];

  const res = await axiosInstance.get(url, { ...config });

  return res.data;
};

export const poster = async (args: string | [string, any]) => {
  const [url, data] = Array.isArray(args) ? args : [args, {}];

  const response = await axiosInstance.post(url, data);
  return response.data;
};

export const endpoints = {
  auth: {
    login: "api/account/login",
    register: "api/account/register",
    retoken: "api/account/refresh-token",
  },
  friend: {
    getList: "api/friend/get-list",
    getListRequest: "api/friend/list-request",
    addFriend: "api/friend/add",
    removeFriend: "api/friend/remove",
    acceptFriend: "api/friend/accept",
    rejectFriend: "api/friend/reject",
    blockFriend: "api/friend/block",
    unblockFriend: "api/friend/unblock",
  },
  chat: {
    getList: "api/chat/get-list",
    createChat: "api/chat/create",
    updateChat: "api/chat/update",
    deleteChat: "api/chat/delete",
    addParticipant: "api/chat/add-participant",
    removeParticipant: "api/chat/remove-participant",
    leaveChat: "api/chat/leave",
    getChatById: "api/chat/get-by-id",
  },
  chatParticipant: {
    getListByChat: "api/chat-participant/list",
    add: "api/chat-participant/add",
    remove: "api/chat-participant/remove",
  },
  message: {
    getListByChat: "api/message/get-list-by-chat",
    send: "api/message/send",
    edit: "api/message/edit",
    delete: "api/message/delete",
  },
  profile: {
    getProfile: "api/profile/get-profile",
  },
  reaction: {
    getListByMessage: "api/reaction/list",
    add: "api/reaction/add",
    update: "api/reaction/update",
    remove: "api/reaction/remove",
  },
};
