import useSWR from "swr";
import { useMemo } from "react";
import { IMessage } from "../types/message";
import axiosInstance, { APIResponse, endpoints, fetcher } from "../utils/axios";

const URL = endpoints.message;

const options = {
  revalidateOnFocus: false,
  revalidateOnReconnect: false,
  revalidateIfStale: false,
  errorRetryCount: 0,
  shouldRetryOnError: false,
  revalidate: 0,
};

export const useListMessageByChat = (chatID: number) => {
  try {
    const { data, error, isLoading, isValidating } = useSWR<
      APIResponse<IMessage[]>
    >(`${URL.getListByChat}/${chatID}`, fetcher, options);

    const memoizedValue = useMemo(() => {
      const messages: IMessage[] = data?.data || [];
      return {
        messages,
        messagesLoading: isLoading,
        messagesError: error,
        messagesValidating: isValidating,
      };
    }, [data, error, isLoading, isValidating]);

    return memoizedValue;
  } catch (error) {
    throw error;
  }
};

// Edit a message by ID
export const editMessage = async (messageId: number, content: string) => {
  try {
    const response = await axiosInstance.post<APIResponse<null>>(
      URL.edit,
      { MessageID: messageId, Content: content }
    );
    return response.data;
  } catch (error) {
    throw error;
  }
};

// Delete a message by ID
export const deleteMessage = async (messageId: number) => {
  try {
    const response = await axiosInstance.post<APIResponse<null>>(
      URL.delete,
      { MessageID: messageId }
    );
    return response.data;
  } catch (error) {
    throw error;
  }
};

