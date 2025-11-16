import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from '@microsoft/signalr';
import { useEffect, useState } from 'react';
import { HOST_CHAT_API } from '@/config-global';

export interface ISocketOnEvent {
  methodName: string;
  newMethod: (...args: any[]) => void;
}

function useSignalR() {
  const [state, setHubState] = useState<HubConnectionState>(HubConnectionState.Connecting);

  const [hubConnection, setHubConnection] = useState<HubConnection>();

  useEffect(() => {
    try {
      if (!hubConnection) {
        const accessToken = localStorage.getItem('accessToken');

        const hub = new HubConnectionBuilder()
          .withUrl(`${HOST_CHAT_API}/chatHub`, {
            withCredentials: false,
            accessTokenFactory: () => accessToken!,
          })
          .withAutomaticReconnect()
          .configureLogging(LogLevel.None)
          .build();

        hub.onclose(() => {
          setHubState(HubConnectionState.Disconnected);
        });

        hub.onreconnecting(() => {
          setHubState(HubConnectionState.Reconnecting);
        });

        hub.onreconnected(async () => {
          setHubState(HubConnectionState.Connected);
        });

        hub
          .start()
          .then(async () => {
            setHubState(HubConnectionState.Connected);
          })
          .catch(() => {
            setHubState(HubConnectionState.Disconnected);
          });

        setHubConnection(hub);
      } else if (state === HubConnectionState.Connected) {
        hubConnection.stop();
        setHubConnection(undefined);
      }
    } catch (e) {
      console.log(e);
    }

    return () => {
      if (hubConnection) {
        hubConnection.stop();
      }
    };
  }, [hubConnection]);

  return { hubState: state, hubConnection };
}

export default useSignalR;
