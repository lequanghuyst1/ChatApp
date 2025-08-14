import { useState } from "react";
import { Grid } from "@mui/material";
import { useGetListChat } from "../../../apis/chat";
import ChatDetail from "../chat-detail";
import ChatList from "../chat-list";
import ChatToolbar from "../chat-toolbar";
import useSignalR from "../hooks/useSignalR";
import { useGetListUser } from "../../../apis/profile";

function ChatView() {
  const { chats, chatsLoading, chatsError, chatsValidating } = useGetListChat();

  const [selectedChat, setSelectedChat] = useState<number | null>(null);

  const { hubState, hubConnection } = useSignalR();

  const { users } = useGetListUser("");

  return (
    <Grid container>
      <Grid item xs={12} xl={1}>
        <ChatToolbar />
      </Grid>

      <Grid item xs={12} md={4} lg={3} xl={2}>
        <ChatList
          chats={chats}
          chatsLoading={chatsLoading}
          chatsError={chatsError}
        />
      </Grid>
      <Grid item xs={12} md={8} lg={9} xl={9}>
        {selectedChat ? (
          <ChatDetail chatID={selectedChat} />
        ) : (
          <h1>ChatDetail</h1>
        )}
      </Grid>
    </Grid>
  );
}

export default ChatView;
