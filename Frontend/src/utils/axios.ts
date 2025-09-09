import axios, { AxiosRequestConfig } from 'axios';
import { HOST_CHAT_API } from '../config-global';

export interface APIResponse<T = any> {
  data: T;
  code: number;
  message: string;
}

const axiosInstance = axios.create({
  baseURL: HOST_CHAT_API,
  headers: {
    'Content-Type': 'application/json',
    Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
  },
});

axiosInstance.interceptors.response.use(
  (response) => response,
  (error) =>
    Promise.reject(
      (error.response && error.response.data) || error.message || 'Something went wrong'
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

// const createEndpoints = (base: string, paths: Record<string, string>) => {
//   const result: Record<string, string> = {};
//   for (const [key, path] of Object.entries(paths)) {
//     result[key] = `${base}/api/${path}`;
//   }
//   return result;
// };

// export const endpoints = {
//   auth: createEndpoints('account', {
//     login: 'login',
//     register: 'register',
//     retoken: 'refresh-token',
//   } as const),
//   friend: createEndpoints('friend', {
//     getList: 'get-list',
//     getListRequest: 'list-request',
//     addFriend: 'add',
//     removeFriend: 'remove',
//     acceptFriend: 'accept',
//     rejectFriend: 'reject',
//     blockFriend: 'block',
//     unblockFriend: 'unblock',
//   }),
//   chat: createEndpoints('chat', {
//     getList: 'get-list',
//     createChat: 'create',
//     updateChat: 'update',
//     deleteChat: 'delete',
//     addParticipant: 'add-participant',
//     removeParticipant: 'remove-participant',
//     leaveChat: 'leave',
//     getChatById: 'get-by-id',
//   }),
//   chatParticipant: createEndpoints('chat-participant', {
//     getListByChat: 'list',
//     add: 'add',
//     remove: 'remove',
//   }),
//   message: createEndpoints('message', {
//     getListByChat: 'get-list-by-chat',
//     send: 'send',
//     edit: 'edit',
//     delete: 'delete',
//   }),
//   profile: createEndpoints('profile', {
//     getProfile: 'get-profile',
//     getUsers: 'get-users',
//   }),
//   reaction: createEndpoints('reaction', {
//     getListByMessage: 'list',
//     add: 'add',
//     update: 'update',
//     remove: 'remove',
//   }),
// };

const BASE_PATHS = {
  account: `api/account`,
  friend: `api/friend`,
  chat: `api/chat`,
  chatParticipant: `api/chat-participant`,
  message: `api/message`,
  profile: `api/profile`,
  reaction: `api/reaction`,
} as const;

// Định nghĩa endpoints
export const endpoints = {
  auth: {
    login: `${BASE_PATHS.account}/login`,
    register: `${BASE_PATHS.account}/register`,
    retoken: `${BASE_PATHS.account}/refresh-token`,
  },
  friend: {
    getList: `${BASE_PATHS.friend}/get-list`,
    getListRequest: `${BASE_PATHS.friend}/list-request`,
    addFriend: `${BASE_PATHS.friend}/add`,
    removeFriend: `${BASE_PATHS.friend}/remove`,
    acceptFriend: `${BASE_PATHS.friend}/accept`,
    rejectFriend: `${BASE_PATHS.friend}/reject`,
    blockFriend: `${BASE_PATHS.friend}/block`,
    unblockFriend: `${BASE_PATHS.friend}/unblock`,
  },
  chat: {
    getList: `${BASE_PATHS.chat}/get-list`,
    createChat: `${BASE_PATHS.chat}/create`,
    updateChat: `${BASE_PATHS.chat}/update`,
    deleteChat: `${BASE_PATHS.chat}/delete`,
    addParticipant: `${BASE_PATHS.chat}/add-participant`,
    removeParticipant: `${BASE_PATHS.chat}/remove-participant`,
    leaveChat: `${BASE_PATHS.chat}/leave`,
    getChatById: `${BASE_PATHS.chat}/get-by-id`,
  },
  chatParticipant: {
    getListByChat: `${BASE_PATHS.chatParticipant}/list`,
    add: `${BASE_PATHS.chatParticipant}/add`,
    remove: `${BASE_PATHS.chatParticipant}/remove`,
  },
  message: {
    getListByChat: `${BASE_PATHS.message}/get-list-by-chat`,
    send: `${BASE_PATHS.message}/send`,
    edit: `${BASE_PATHS.message}/edit`,
    delete: `${BASE_PATHS.message}/delete`,
  },
  profile: {
    getProfile: `${BASE_PATHS.profile}/get-profile`,
    getUsers: `${BASE_PATHS.profile}/get-users`,
  },
  reaction: {
    getListByMessage: `${BASE_PATHS.reaction}/list`,
    add: `${BASE_PATHS.reaction}/add`,
    update: `${BASE_PATHS.reaction}/update`,
    remove: `${BASE_PATHS.reaction}/remove`,
  },
} as const;
