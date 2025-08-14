import { IMessage } from "../../types/message";

type Props = {
  messages: IMessage[];
};

function MessageList({ messages }: Props) {
  return (
    <div>
      {messages.map((message) => (
        <div key={message.id}>{message.content}</div>
      ))}
    </div>
  );
}

export default MessageList;
