import {
  Avatar,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  ListItem,
  ListItemAvatar,
  ListItemButton,
  ListItemText,
  Stack,
  Typography,
  Tabs,
  Tab,
} from '@mui/material';
import { useCallback, useState } from 'react';
import { useGetListUser } from '@/apis/profile';
import {
  acceptFriend,
  addRequestFriend,
  rejectFriend,
  useGetListFriendRequest,
} from '@/apis/friend';

interface Props {
  open: boolean;
  onClose: () => void;
}

const TABS = [
  {
    value: 'users',
    label: 'Danh sách người dùng',
  },
  {
    value: 'requests',
    label: 'Yêu cầu kết bạn',
  },
];

function AddFriendDialog({ open, onClose }: Props) {
  const { users } = useGetListUser('');

  const [value, setValue] = useState('users');

  const { friendsRequest } = useGetListFriendRequest();

  const handleChange = (event: React.SyntheticEvent, newValue: string) => {
    setValue(newValue);
  };

  const handleAddFriend = useCallback(async (userID: number) => {
    try {
      const { code, message } = await addRequestFriend(userID);
      if (code === 1) {
        onClose();
      }
    } catch (error) {
      console.error(error);
    }
  }, []);

  const handleAcceptFriend = useCallback(async (friendID: number) => {
    try {
      const { code, message } = await acceptFriend(friendID);
      if (code === 1) {
        onClose();
      }
    } catch (error) {
      console.error(error);
    }
  }, []);

  const handleRejectFriend = useCallback(async (friendID: number) => {
    try {
      const { code, message } = await rejectFriend(friendID);
      if (code === 1) {
        onClose();
      }
    } catch (error) {
      console.error(error);
    }
  }, []);

  return (
    <Dialog fullWidth maxWidth="md" open={open} onClose={onClose}>
      <DialogTitle>Thêm bạn bè</DialogTitle>
      <Tabs value={value} onChange={handleChange}>
        {TABS.map((tab) => (
          <Tab key={tab.value} label={tab.label} value={tab.value} />
        ))}
      </Tabs>
      <DialogContent>
        {value === 'users' && (
          <Stack>
            {users.map((user) => (
              <ListItem key={user.userID}>
                <ListItemAvatar>
                  <Avatar alt={user.avatar} src={user.avatar} />
                </ListItemAvatar>
                <ListItemText>
                  <Typography>{user.fullname}</Typography>
                  <Typography>{user.email ? user.email : user.phone}</Typography>
                </ListItemText>
                <ListItemButton>
                  <Button onClick={() => handleAddFriend(user.userID)}>Thêm</Button>
                </ListItemButton>
              </ListItem>
            ))}
          </Stack>
        )}

        {value === 'requests' && (
          <Stack>
            {friendsRequest.map((friend) => (
              <ListItem key={friend.id}>
                <ListItemAvatar>
                  <Avatar alt={friend.friendAvatar} src={friend.friendAvatar} />
                </ListItemAvatar>
                <ListItemText>
                  <Typography>{friend.friendName}</Typography>
                </ListItemText>
                <ListItemButton>
                  <Button onClick={() => handleAcceptFriend(friend.id)}>Xác nhận</Button>
                  <Button onClick={() => handleRejectFriend(friend.id)}>Từ chối</Button>
                </ListItemButton>
              </ListItem>
            ))}
          </Stack>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Hủy</Button>
      </DialogActions>
    </Dialog>
  );
}

export default AddFriendDialog;
