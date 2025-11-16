import { useState } from 'react';
import { Box, Grid } from '@mui/material';
import { useGetListChat } from '@/apis/chat';
import { useBoolean } from '@/hooks/use-boolean';
import { IChat } from '@/types/chat';
import ChatDetail from '../chat-detail';
import ChatList from '../chat-list';
import ChatToolbar from '../chat-toolbar';
// import useSignalR from "../hooks/useSignalR";
import AddFriendDialog from '../dialogs/add-friend-dialog';

function ChatView() {
  const { chats, chatsLoading, chatsError } = useGetListChat();

  const [selectedChat, setSelectedChat] = useState<IChat | null>(null);

  // const { hubState, hubConnection } = useSignalR();

  const openAddFriendDialog = useBoolean();

  return (
    <Box sx={{ display: 'flex', background: 'black', height: '100vh', p: 2 }}>
      <Grid container spacing={2}>
        <Grid item xs={12} lg={1} xl={0.5}>
          <ChatToolbar onAddFriend={openAddFriendDialog.onTrue} />
        </Grid>

        <Grid item xs={12} md={4} lg={4} xl={2.5}>
          <ChatList
            chats={chats}
            selectedChatID={selectedChat?.id || 0}
            chatsLoading={chatsLoading}
            chatsError={chatsError}
            onChatSelect={setSelectedChat}
          />
        </Grid>
        <Grid item xs={12} md={8} lg={7} xl={9}>
          {selectedChat ? <ChatDetail chatID={selectedChat.id} /> : <h1>ChatDetail</h1>}
        </Grid>
      </Grid>

      {/* Dialog danh sách user và thêm bạn bè */}
      <AddFriendDialog open={openAddFriendDialog.value} onClose={openAddFriendDialog.onFalse} />
    </Box>
  );
}

export default ChatView;
