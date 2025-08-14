import useSWR, { mutate } from "swr";
import { useMemo } from "react";
import { APIResponse, endpoints, fetcher } from "../utils/axios";
import { IUserProfile } from "../types/profile";

const URL = endpoints.profile;

const options = {
  revalidateOnFocus: false,
  revalidateOnReconnect: false,
  revalidateIfStale: false,
  errorRetryCount: 0,
  shouldRetryOnError: false,
  revalidate: 0,
};

export const useGetListUser = (keyword: string) => {
  try {
    const URL_FETCH = `${URL.getUsers}?keyword=${keyword}`;

    const { data, error, isLoading, isValidating } = useSWR<
      APIResponse<IUserProfile[]>
    >(URL_FETCH, fetcher, options);

    const memoizedValue = useMemo(() => {
      const users: IUserProfile[] = data?.data || [];
      return {
        users,
        usersLoading: isLoading,
        usersError: error,
        usersValidating: isValidating,
      };
    }, [data, isLoading, error, isValidating]);

    return memoizedValue;
  } catch (error) {
    console.error("Error fetching profile:", error);
    throw error;
  }
};
