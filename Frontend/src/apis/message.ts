import { IMessage } from "../types/message";
import { APIResponse, endpoints, fetcher } from "../utils/axios";
import useSWR from "swr";
import { useMemo } from "react";

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

