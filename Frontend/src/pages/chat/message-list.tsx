import { List, ListItem, ListItemText } from "@mui/material";
import { IMessage } from "../../types/message";
import { useAuthContext } from "../../stores/auth";

type Props = {
  messages: IMessage[];
};

function MessageList({ messages }: Props) {
  const { user } = useAuthContext();
  return (
    <List>
      {messages.map((msg) => (
        <ListItem
          key={msg.id}
          sx={{
            justifyContent:
              msg.senderID === user?.userID ? "flex-end" : "flex-start",
            my: "0",
            py: "0.1rem",
          }}
        >
          <ListItemText
            primary={msg.content}
            sx={{
              bgcolor: msg.senderID === user?.userID ? "#4a90e2" : "#333",
              color: "#fff",
              borderRadius: 1,
              py: 0.5,
              px: 1.5,
              flex: "0 0 auto",
            }}
          />
        </ListItem>
      ))}
    </List>
  );
}

export default MessageList;
