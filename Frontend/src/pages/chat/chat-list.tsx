import { IChat } from "../../types/chat";
import { Avatar, Card, Stack, Typography } from "@mui/material";

type Props = {
  chats: IChat[];
  chatsLoading: boolean;
  chatsError: boolean;
  onChatSelect: (chat: IChat) => void;
};

function ChatList({ chats, chatsLoading, chatsError, onChatSelect }: Props) {
  if (chatsLoading) {
    return <div>Loading...</div>;
  }

  if (chatsError) {
    return <div>Error: {chatsError}</div>;
  }

  return (
    <Card sx={{ backgroundColor: "#1f1f1f", height: "100%", borderRadius: 1 }}>
      <Stack>
        {chats.map((chat) => (
          <Stack
            key={chat.id}
            direction="row"
            alignItems="center"
            spacing={2}
            sx={{
              p: 1.5,
              cursor: "pointer",
              transition: "all 0.2s ease-in-out",
              "&:hover": {
                backgroundColor: "#2f2f2f",
                borderRadius: 1,
              },
            }}
            onClick={() => onChatSelect(chat)}
          >
            <Avatar />
            <Typography color="white">{chat.createdByName}</Typography>
          </Stack>
        ))}
      </Stack>
    </Card>
  );
}

export default ChatList;
