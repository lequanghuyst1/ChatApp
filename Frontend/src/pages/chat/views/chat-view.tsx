import { Grid } from "@mui/material";
import useSignalR from "./hooks/useSignalR";
import { useGetListChat } from "../../../apis/chat";
import { HOST_CHAT_API } from "../../../config-global";


function ChatView() {
  const { hubState, hubConnection } = useSignalR(`${HOST_CHAT_API}/chatHub`);

  const { chats, chatsLoading, chatsError, chatsValidating } = useGetListChat();

  return (
    <Grid container>
      <Grid item xs={12} xl={1}>
        1
      </Grid>
      <Grid item xs={12} md={4} lg={3} xl={2}>
        <h1>ChatList</h1>
      </Grid>
      <Grid item xs={12} md={8} lg={9} xl={9}>
        <h1>ChatDetail</h1>
      </Grid>
    </Grid>
  );
}

export default ChatView;



