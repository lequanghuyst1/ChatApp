import { Box, Stack } from "@mui/material";
import Iconify from "@/components/iconify";

interface Props {
    onAddFriend: () => void   
}

function ChatToolbar({ onAddFriend }: Props) {
  return (
    <Stack
      alignItems="center"
      sx={{ height: "100%", borderRadius: 1, backgroundColor: "transparent" }}
    >
      <Box
        onClick={onAddFriend}
        sx={{
          borderRadius: 1,
          backgroundColor: "#1f1f1f",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          p: 1.5,
          cursor: "pointer",
          transition: "all 0.2s ease-in-out",
          "&:hover": {
            backgroundColor: "#3f3f3f",
          },
        }}
      >
        <Iconify icon="mi:user-add" width={24} height={24} color="white" />
      </Box>
    </Stack>
  );
}

export default ChatToolbar;
