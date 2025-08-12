import { FriendStatus } from "./enums";

export interface IFriend {
  id: number;
  userID: number;
  friendID: number;
  friendName: string;
  friendAvatar: string;
  status: FriendStatus;
  addedAt: Date;
}