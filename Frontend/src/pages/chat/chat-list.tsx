import { IChat } from "../../types/chat";

type Props = {
  chats: IChat[];
  chatsLoading: boolean;
  chatsError: boolean;
};

function ChatList({ chats, chatsLoading, chatsError }: Props) {
  if (chatsLoading) {
    return <div>Loading...</div>;
  }

  if (chatsError) {
    return <div>Error: {chatsError}</div>;
  }

  return <div>ChatList</div>;
}

export default ChatList;
