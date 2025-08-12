export interface IReaction {
  id: number;
  messageID: number;
  senderID: number;
  senderName: string;
  senderAvatar: string;
  emoji: string;
  createdAt: Date;
}
