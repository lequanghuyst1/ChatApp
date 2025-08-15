import { useCallback, useEffect, useState } from "react";
import { HubConnectionState } from "@microsoft/signalr";
import { useGetChatDetail, useGetListChat } from "../../apis/chat";
import { IMessage } from "../../types/message";
import MessageList from "./message-list";
import useSignalR, { ISocketOnEvent } from "./hooks/useSignalR";
import { useGetListMessageByChat } from "../../apis/message";

type Props = {
  chatID: number;
};

function ChatDetail({ chatID }: Props) {
  const { chat, chatLoading, chatError, chatValidating } =
    useGetChatDetail(chatID);

  const {
    messages: listMessages,
    messagesLoading,
    messagesError,
    messagesValidating,
  } = useGetListMessageByChat(chatID, 1, 20);

  const [messages, setMessages] = useState<IMessage[]>([]);

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

  useEffect(() => {
    if (listMessages) {
      setMessages(listMessages);
    }
  }, [listMessages]);

  useEffect(() => {
    if (hubState === HubConnectionState.Connected) {
      registerCallback({
        methodName: "ReceiveMessage",
        newMethod: (message: IMessage) => {
          setMessages((prevMessages) => [...prevMessages, message]);
        },
      });
    }
  }, [hubState, registerCallback]);

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
    <div>
      <MessageList messages={messages} />
    </div>
  );
}

export default ChatDetail;
