import {
  Avatar,
  Box,
  List,
  ListItem,
  ListItemText,
  Stack,
  styled,
  Typography,
} from '@mui/material';
import React, { useRef, useEffect, useCallback } from 'react';
import { format } from 'date-fns';
import vi from 'date-fns/locale/vi';
import { IMessage } from '@/types/message';
import { useAppSelector } from '@/stores/hook';

type Props = {
  messages: IMessage[];
  totalRec: number;
  isLoading: boolean;
  error: any;
  onLoadMore: () => void;
  hasMore: boolean;
};

interface MessageGroup {
  senderID: number;
  senderName: string;
  senderAvatar: string | null;
  messages: IMessage[];
}

function groupMessages(messages: IMessage[]): MessageGroup[] {
  const groups: MessageGroup[] = [];

  for (const msg of messages) {
    const lastGroup = groups[groups.length - 1];

    if (lastGroup && lastGroup.senderID === msg.senderID) {
      // cùng người gửi => push vào group hiện tại
      lastGroup.messages.push(msg);
    } else {
      // khác người => tạo group mới
      groups.push({
        senderID: msg.senderID,
        senderName: msg.senderName,
        senderAvatar: msg.senderAvatar,
        messages: [msg],
      });
    }
  }
  return groups;
}

// Hàm check khác ngày
const isDifferentDay = (d1: Date, d2: Date) => {
  if (!d1 || !d2) return true;
  const date1 = new Date(d1);
  const date2 = new Date(d2);
  return (
    date1.getFullYear() !== date2.getFullYear() ||
    date1.getMonth() !== date2.getMonth() ||
    date1.getDate() !== date2.getDate()
  );
};

function MessageList({
  messages,
  totalRec: _totalRec,
  isLoading,
  error: _error,
  onLoadMore,
  hasMore,
}: Props) {
  const { user } = useAppSelector((state) => state.auth);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const listRef = useRef<HTMLDivElement>(null);
  const prevScrollHeight = useRef<number>();
  const loadingRef = useRef(false);

  const grouped = groupMessages(messages);

  const isSender = (senderID: number) => senderID === user?.userID;

  // Handle scroll to load more messages
  const handleScroll = useCallback(() => {
    if (!listRef.current || isLoading || !hasMore) return;

    const { scrollTop } = listRef.current;
    if (scrollTop < 100 && !loadingRef.current) {
      loadingRef.current = true;
      prevScrollHeight.current = listRef.current.scrollHeight - listRef.current.scrollTop;
      onLoadMore();
    }
  }, [isLoading, onLoadMore, hasMore]);

  // Reset loading ref when messages change
  useEffect(() => {
    loadingRef.current = false;
  }, [messages]);

  // Maintain scroll position when loading older messages
  useEffect(() => {
    if (listRef.current && prevScrollHeight.current) {
      listRef.current.scrollTop = listRef.current.scrollHeight - prevScrollHeight.current;
    } else if (messagesEndRef.current) {
      messagesEndRef.current.scrollIntoView({ behavior: 'smooth' });
    }
  }, [messages]);

  return (
    <Box
      ref={listRef}
      onScroll={handleScroll}
      sx={{
        pl: 1.5,
        height: '100%',
        overflowY: 'auto',
        '&::-webkit-scrollbar': {
          width: '6px',
        },
        '&::-webkit-scrollbar-thumb': {
          backgroundColor: 'rgba(0,0,0,0.2)',
          borderRadius: '3px',
        },
      }}
    >
      {isLoading && hasMore && (
        <Box sx={{ textAlign: 'center', py: 2 }}>
          <Typography variant="body2" color="text.secondary">
            Đang tải thêm tin nhắn...
          </Typography>
        </Box>
      )}
      {grouped.map((item, index) => (
        <Stack
          direction="row"
          key={`${item.senderID}-${index}`}
          sx={{ width: '100%' }}
          alignItems="flex-end"
          justifyContent={isSender(item.senderID) ? 'flex-end' : 'flex-start'}
        >
          {!isSender(item.senderID) && (
            <Avatar src={item.senderAvatar || ''} sx={{ width: 32, height: 32 }} />
          )}
          <List sx={{ py: 0, flex: 1 }}>
            {item.messages.map((msg, msIndex) => {
              // Lấy message trước đó (để so sánh ngày)
              const prevMsg =
                index === 0 && msIndex === 0
                  ? null
                  : msIndex > 0
                    ? item.messages[msIndex - 1]
                    : grouped[index - 1]?.messages.slice(-1)[0];

              const showDate = !prevMsg || isDifferentDay(msg.createdAt, prevMsg.createdAt);

              const formatString = "HH:mm dd 'Tháng' M, yyyy";

              return (
                <React.Fragment key={msg.id}>
                  {showDate && (
                    <Stack
                      direction="row"
                      justifyContent="center"
                      alignItems="center"
                      sx={{ my: 1, flex: 1 }}
                    >
                      <Typography variant="caption" sx={{ color: 'gray', fontSize: '0.75rem' }}>
                        {format(new Date(msg.createdAt), formatString, {
                          locale: vi,
                        })}
                      </Typography>
                    </Stack>
                  )}

                  <ListItem
                    sx={{
                      justifyContent: isSender(msg.senderID) ? 'flex-end' : 'flex-start',
                      my: '0',
                      py: '0.1rem',
                      px: '0.6rem',
                    }}
                  >
                    <MessageTextStyle
                      primary={<Typography variant="body1">{msg.content}</Typography>}
                      isSender={isSender(msg.senderID)}
                      isFirst={item.messages.length > 1 && msIndex === 0}
                      isLast={item.messages.length > 1 && msIndex === item.messages.length - 1}
                      isOnly={item.messages.length === 1}
                    />
                  </ListItem>
                </React.Fragment>
              );
            })}
          </List>
        </Stack>
      ))}
      <div ref={messagesEndRef} />
    </Box>
  );
}

export default MessageList;

type MessageTextStyleProps = {
  isSender: boolean;
  isFirst: boolean;
  isLast: boolean;
  isOnly: boolean;
};

const getBorderRadii = (props: MessageTextStyleProps) => {
  const { isSender, isFirst, isLast, isOnly } = props;
  // Default border radii for all corners
  const radii = {
    borderTopLeftRadius: '16px',
    borderTopRightRadius: '16px',
    borderBottomLeftRadius: '16px',
    borderBottomRightRadius: '16px',
  };

  if (isOnly) return radii; // All corners rounded for single messages

  if (isSender) {
    if (isFirst) {
      return {
        ...radii,
        borderBottomRightRadius: '2px',
      };
    }
    if (isLast) {
      return {
        ...radii,
        borderTopRightRadius: '2px',
      };
    }
    return {
      ...radii,
      borderTopRightRadius: '2px',
      borderBottomRightRadius: '2px',
    };
  }

  // Non-sender messages
  if (isFirst) {
    return {
      ...radii,
      borderBottomLeftRadius: '2px',
    };
  }
  if (isLast) {
    return {
      ...radii,
      borderTopLeftRadius: '2px',
    };
  }
  return {
    ...radii,
    borderTopLeftRadius: '2px',
    borderBottomLeftRadius: '2px',
  };
};

const MessageTextStyle = styled(ListItemText, {
  shouldForwardProp: (prop) =>
    !['isSender', 'isFirst', 'isLast', 'isOnly'].includes(prop as string),
})<MessageTextStyleProps>(({ isSender, isFirst, isLast, isOnly }) => ({
  backgroundColor: isSender ? '#4a90e2' : '#333',
  color: '#fff',
  padding: '6px 12px',
  flex: '0 0 auto',
  ...getBorderRadii({ isSender, isFirst, isLast, isOnly }),
}));
