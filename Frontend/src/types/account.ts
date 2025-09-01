import { AccountType } from "./enums";
import { IUserProfile } from "./profile";

export interface IAccount {
  UserID: number;
  Username: string;
  AccountType: AccountType;
  CreatedTime: Date;
  IsActive: boolean;
  IsOnline: boolean;
  LastSeen: Date;
}

export interface ILoginResponse {
  accessToken: string;
  refreshToken: string;
  userProfile: IUserProfile;
}

export interface ILoginRequest {
  username: string;
  password: string;
}

export interface IRegisterRequest {
  firstName: string;
  lastName: string;
  username: string;
  password: string;
}

export interface IRegisterResponse {
  accessToken: string;
  refreshToken: string;
  userProfile: IUserProfile;
}
