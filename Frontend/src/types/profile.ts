export interface IUserProfile {
  userID: number;
  firstName: string;
  lastName: string;
  fullname: string;
  avatar: string;
  dateOfBirth: Date | null;
  phone: string;
  email: string;
  gender: number;
  bio: string;
}
