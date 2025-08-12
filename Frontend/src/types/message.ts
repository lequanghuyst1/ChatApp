import { MessageType } from "./enums";

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