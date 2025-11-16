import { ChatType } from './enums';
import { IChatParticipant } from './chat-participant';
import { IMessage } from './message';

export interface IChat {
  id: number;
  type: ChatType;
  title: string;
  createdBy: number;
  createdByName: string;
  createdAt: Date;
  updatedBy: number;
  updatedByName: string;
  updatedAt: Date;
  isDeleted: boolean;
  participants: IChatParticipant[];
  messages: IMessage[];
}
