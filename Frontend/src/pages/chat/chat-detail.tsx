import { KeyboardEvent, useCallback, useEffect, useState } from "react";
import { HubConnectionState } from "@microsoft/signalr";
import { useGetChatDetail, useGetListChat } from "../../apis/chat";
import { IMessage, ISearchMessage } from "../../types/message";
import MessageList from "./message-list";
import useSignalR, { ISocketOnEvent } from "./hooks/useSignalR";
import { useGetListMessageByChat } from "../../apis/message";
import { MessageType } from "../../types/enums";
import {
  Avatar,
  Box,
  Card,
  IconButton,
  inputBaseClasses,
  outlinedInputClasses,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { useAuthContext } from "../../stores/auth";

type Props = {
  chatID: number;
};

function ChatDetail({ chatID }: Props) {
  const { user } = useAuthContext();

  const { chat, chatLoading, chatError, chatValidating } =
    useGetChatDetail(chatID);

  const [filters, setFilters] = useState<ISearchMessage>({
    chatID,
    page: 1,
    pageSize: 20,
  });

  const [hasMore, setHasMore] = useState(true);

  const {
    messages: listMessages,
    messagesLoading,
    messagesError,
    messagesValidating,
    totalRec,
  } = useGetListMessageByChat(filters);

  const [messages, setMessages] = useState<IMessage[]>([]);

  const [message, setMessage] = useState<string>("");

  const { hubState, hubConnection } = useSignalR();

  const registerCallback = useCallback(
    (event: ISocketOnEvent) => {
      hubConnection?.on(event.methodName, event.newMethod);
    },
    [hubConnection]
  );

  const removeCallback = useCallback(
    (event: ISocketOnEvent) => {
      hubConnection?.off(event.methodName, event.newMethod);
    },
    [hubConnection]
  );

  // Reset messages when chatID changes
  useEffect(() => {
    setMessages([]);
    setFilters((prev) => ({
      ...prev,
      page: 1,
      chatID,
    }));
    setHasMore(true);
  }, [chatID]);

  const loadMoreMessages = useCallback(() => {
    if (messages.length >= totalRec) {
      setHasMore(false);
      return;
    }
    setFilters((prev) => ({
      ...prev,
      page: prev.page + 1,
    }));
  }, [messages.length, totalRec]);

  // Merge new messages with existing ones
  useEffect(() => {
    if (listMessages) {
      if (filters.page === 1) {
        setMessages(listMessages);
      } else {
        setMessages((prev) => [...prev, ...listMessages]);
      }
    }
  }, [listMessages, filters.page]);

  useEffect(() => {
    if (hubState === HubConnectionState.Connected && chatID) {
      hubConnection
        ?.invoke("JoinChat", chatID)
        .catch((err) => console.error("JoinChat error:", err));
    }
  }, [hubState, chatID, hubConnection]);

  useEffect(() => {
    if (hubState === HubConnectionState.Connected) {
      const eventReceiveMessage: ISocketOnEvent = {
        methodName: "ReceiveMessage",
        newMethod: (message: IMessage) => {
          console.log(message);
          setMessages((prevMessages) => [...prevMessages, message]);
        },
      };

      registerCallback(eventReceiveMessage);

      return () => {
        removeCallback(eventReceiveMessage);
      };
    }

    return () => {};
  }, [hubState, registerCallback, removeCallback]);

  const sendMessage = async (message: string, type: MessageType) => {
    if (hubState === HubConnectionState.Connected) {
      hubConnection?.invoke("SendMessage", chatID, message, type);
      setMessage("");
    }
  };

  const handleKeyPress = (event: KeyboardEvent<HTMLDivElement>) => {
    if (event.key === "Enter" && !event.shiftKey) {
      event.preventDefault();
      sendMessage(message, MessageType.TEXT);
    }
  };

  if (chatLoading) {
    return <div>Loading...</div>;
  }

  if (chatError) {
    return <div>Error: {chatError.message}</div>;
  }

  if (chatValidating) {
    return <div>Validating...</div>;
  }

  return (
    <Card
      sx={{
        backgroundColor: "#1f1f1f",
        height: "calc(100vh - 32px)",
        borderRadius: 1,
      }}
    >
      <Stack
        sx={{ flex: 1, height: "100%", pr: 1 }}
        justifyContent="space-between"
      >
        <Stack
          spacing={2}
          direction="row"
          alignItems="center"
          sx={{ padding: "10px 12px" }}
        >
          <Avatar />
          <Typography color="white">
            {chat.participants.find((p) => p.userID !== user?.userID)?.fullName}
          </Typography>
        </Stack>

        <Box
          sx={{
            flex: 1,
            height: "calc(100% - 64px)",
            overflowY: "auto", // hoặc "scroll"
            mb: 2,
            // 👇 custom scrollbar
            "&::-webkit-scrollbar": {
              width: "10px", // độ rộng scrollbar
            },
            "&::-webkit-scrollbar-track": {
              background: "transparent", // nền track trong suốt
            },
            "&::-webkit-scrollbar-thumb": {
              backgroundColor: "rgba(255,255,255,0.3)", // màu thanh kéo
              borderRadius: "10px",
            },
            "&::-webkit-scrollbar-thumb:hover": {
              backgroundColor: "rgba(255,255,255,0.5)",
            },
          }}
        >
          <MessageList
            messages={messages}
            totalRec={totalRec}
            isLoading={messagesLoading}
            error={messagesError}
            onLoadMore={loadMoreMessages}
            hasMore={hasMore && messages.length < totalRec}
          />
        </Box>

        <Box sx={{ display: "flex", alignItems: "center", padding: "12px 0" }}>
          <TextField
            fullWidth
            variant="outlined"
            placeholder="Aa"
            value={message}
            onChange={(e) => setMessage(e.target.value)}
            onKeyDown={handleKeyPress}
            size="small"
            sx={{
              bgcolor: "#3a3b3c",
              color: "#fff",
              borderRadius: 3,
              [`& .${outlinedInputClasses.root}`]: {
                borderRadius: 3,
              },
              [`& .${outlinedInputClasses.notchedOutline}`]: {
                borderColor: "transparent",
              },
              [`& .${inputBaseClasses.input}`]: {
                caretColor: "#fff", // Đổi màu cursor thành xanh lá khi focus
                color: "#fff",
              },
              [`& .${inputBaseClasses.focused}`]: {
                caretColor: "#fff", // Đổi màu cursor thành đỏ khi input được focus
              },
            }}
          />
        </Box>
      </Stack>
    </Card>
  );
}

export default ChatDetail;
