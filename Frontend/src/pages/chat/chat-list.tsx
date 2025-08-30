import { Avatar, Card, Stack, Typography } from "@mui/material";
import { useAuthContext } from "@/stores/auth";
import { IChat } from "@/types/chat";

type Props = {
  chats: IChat[];
  selectedChatID: number;
  chatsLoading: boolean;
  chatsError: boolean;
  onChatSelect: (chat: IChat) => void;
};

function ChatList({
  chats,
  selectedChatID,
  chatsLoading,
  chatsError,
  onChatSelect,
}: Props) {
  const { user } = useAuthContext();
  if (chatsLoading) {
    return <div>Loading...</div>;
  }

  if (chatsError) {
    return <div>Error: {chatsError}</div>;
  }

  return (
    <Card
      sx={{
        backgroundColor: "#1f1f1f",
        height: "100%",
        borderRadius: 1,
        px: 1,
      }}
    >
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
              ...(chat.id === selectedChatID && {
                backgroundColor: "#2f2f2f",
                borderRadius: 1,
              }),
            }}
            onClick={() => onChatSelect(chat)}
          >
            <Avatar />
            <Typography color="white">
              {
                chat.participants.find((p) => p.userID !== user?.userID)
                  ?.fullName
              }
            </Typography>
          </Stack>
        ))}
      </Stack>
    </Card>
  );
}

export default ChatList;

