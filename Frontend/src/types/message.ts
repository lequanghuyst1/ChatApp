import { MessageType } from "./enums";
import { IPagination } from "./common";

export interface ISearchMessage extends IPagination {
  chatID: number;
}

export interface IMessage {
  id: number;
  chatID: number;
  senderID: number;
  senderName: string;
  senderAvatar: string;
  content: string;
  messageType: MessageType;
  createdAt: Date;
  isEdited: boolean;
  editedAt: Date | null;
  isDeleted: boolean;
  deletedAt: Date | null;
  readBy: string;
}
