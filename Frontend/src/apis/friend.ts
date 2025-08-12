import axiosInstance, { endpoints, fetcher } from "../utils/axios";
import { IFriend } from "../types/friend";
import useSWR, { mutate } from "swr";
import { useMemo } from "react";
import { APIResponse } from "../utils/axios";

const URL = endpoints.friend;

const options = {
  revalidateOnFocus: false,
  revalidateOnReconnect: false,
  revalidateIfStale: false,
  errorRetryCount: 0,
  shouldRetryOnError: false,
  revalidate: 0,
};

export const useListFriend = () => {
  try {
    const { data, error, isLoading, isValidating } = useSWR<
      APIResponse<IFriend[]>
    >(`${URL.getList}`, fetcher, options);

    const memoizedValue = useMemo(() => {
      const friends: IFriend[] = data?.data || [];
      return {
        friends,
        friendsLoading: isLoading,
        friendsError: error,
        friendsValidating: isValidating,
      };
    }, [data, error, isLoading, isValidating]);

    return memoizedValue;
  } catch (error) {
    throw error;
  }
};

export const useListFriendRequest = () => {
  try {
    const { data, error, isLoading, isValidating } = useSWR<
      APIResponse<IFriend[]>
    >(`${URL.getListRequest}`, fetcher, options);

    const memoizedValue = useMemo(() => {
      const friendsRequest: IFriend[] = data?.data || [];
      return {
        friendsRequest,
        friendsRequestLoading: isLoading,
        friendsRequestError: error,
        friendsRequestValidating: isValidating,
      };
    }, [data, error, isLoading, isValidating]);

    return memoizedValue;
  } catch (error) {
    throw error;
  }
};

export const addRequestFriend = async (friendID: number) => {
  try {
    const res = await axiosInstance.post<APIResponse<number>>(
      `${URL.addFriend}`,
      { FriendID: friendID }
    );

    const { data, code, message } = res.data;

    return { data, code, message };
  } catch (error) {
    throw error;
  }
};

export const acceptFriend = async (friendID: number) => {
  try {
    const res = await axiosInstance.post<APIResponse<number>>(
      `${URL.acceptFriend}`,
      { FriendID: friendID }
    );

    const { data, code, message } = res.data;

    if (code === 1) {
      mutate(`${URL.getList}`);
      mutate(`${URL.getListRequest}`);
    }

    return { data, code, message };
  } catch (error) {
    throw error;
  }
};

export const rejectFriend = async (friendID: number) => {
  try {
    const res = await axiosInstance.post<APIResponse<number>>(
      `${URL.rejectFriend}`,
      { FriendID: friendID }
    );

    const { data, code, message } = res.data;

    if (code === 1) {
      mutate(`${URL.getList}`);
      mutate(`${URL.getListRequest}`);
    }

    return { data, code, message };
  } catch (error) {
    throw error;
  }
};

export const blockFriend = async (friendID: number) => {
  try {
    const res = await axiosInstance.post<APIResponse<number>>(
      `${URL.blockFriend}`,
      { FriendID: friendID }
    );

    const { data, code, message } = res.data;

    if (code === 1) {
      mutate(`${URL.getList}`, (currentData: any) => {
        const newData = currentData?.data?.filter(
          (friend: any) => friend.friendID !== friendID
        );
        return { ...currentData, data: newData };
      });
    }

    return { data, code, message };
  } catch (error) {
    throw error;
  }
};

export const unblockFriend = async (friendID: number) => {
  try {
    const res = await axiosInstance.post<APIResponse<number>>(
      `${URL.unblockFriend}`,
      { FriendID: friendID }
    );

    const { data, code, message } = res.data;

    if (code === 1) {
      mutate(`${URL.getList}`);
    }

    return { data, code, message };
  } catch (error) {
    throw error;
  }
};
