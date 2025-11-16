import { ChatParticipantType } from './enums';

export interface IChatParticipant {
  id: number;
  chatID: number;
  userID: number;
  fullName: string;
  avatar: string;
  role: ChatParticipantType;
  joinedAt: Date;
}
