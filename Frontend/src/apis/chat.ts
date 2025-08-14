import useSWR, { mutate } from "swr";
import { useMemo } from "react";
import axiosInstance, { APIResponse } from "../utils/axios";
import { IChat } from "../types/chat";
import { fetcher, endpoints } from "../utils/axios";

const URL = endpoints.chat;

const options = {
  revalidateOnFocus: false,
  revalidateOnReconnect: false,
  revalidateIfStale: false,
  errorRetryCount: 0,
  shouldRetryOnError: false,
  revalidate: 0,
};

export const useGetListChat = () => {
  try {
    const { data, error, isLoading, isValidating } = useSWR<
      APIResponse<IChat[]>
    >(URL.getList, fetcher, options);

    const memoizedValue = useMemo(() => {
      const chats: IChat[] = data?.data || [];
      return {
        chats,
        chatsLoading: isLoading,
        chatsError: error,
        chatsValidating: isValidating,
      };
    }, [data, error, isLoading, isValidating]);

    return memoizedValue;
  } catch (error) {
    throw error;
  }
};

export const useGetChatDetail = (chatID: number) => {
  try {
    const URL_FETCH = `${URL.getChatById}/${chatID}`;

    const { data, error, isLoading, isValidating } = useSWR(
      URL_FETCH,
      fetcher,
      options
    );

    const memoizedValue = useMemo(() => {
      const chat: IChat = data?.data || {};

      return {
        chat,
        chatLoading: isLoading,
        chatError: error,
        chatValidating: isValidating,
      };
    }, [data, error, isLoading, isValidating]);

    return memoizedValue;
  } catch (error) {
    throw error;
  }
};

export const leaveChat = async (chatID: number) => {
  try {
    const response = await axiosInstance.post<APIResponse<number>>(
      `${URL.leaveChat}`,
      { ChatID: chatID }
    );

    const { data, code, message } = response.data;

    if (code === 1) {
      mutate(
        `${URL.getList}`,
        (currentData: any) => {
          const newData = currentData?.data?.filter(
            (chat: any) => chat.chatID !== chatID
          );
          return { ...currentData, data: newData };
        },
        false
      );
    }

    return { data, code, message };
  } catch (error) {
    throw error;
  }
};

export const deleteChat = async (chatID: number) => {
  try {
    const response = await axiosInstance.post<APIResponse<number>>(
      `${URL.deleteChat}`,
      { ChatID: chatID }
    );

    const { data, code, message } = response.data;

    if (code === 1) {
      mutate(
        `${URL.getList}`,
        (currentData: any) => {
          const newData = currentData?.data?.filter(
            (chat: any) => chat.chatID !== chatID
          );
          return { ...currentData, data: newData };
        },
        false
      );
    }

    return { data, code, message };
  } catch (error) {
    throw error;
  }
};
