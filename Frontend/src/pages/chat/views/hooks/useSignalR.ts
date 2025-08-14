import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from "@microsoft/signalr";
import { useEffect, useState } from "react";

export interface ISocketOnEvent {
  methodName: string;
  newMethod: (...args: any[]) => void;
}

function useSignalR(url: string, events?: ISocketOnEvent[]) {
  const [state, setHubState] = useState<HubConnectionState>(
    HubConnectionState.Connecting
  );
  const [hubConnection, setHubConnection] = useState<HubConnection>();

  useEffect(() => {
    try {
      if (!hubConnection) {
        const accessToken = localStorage.getItem("accessToken");

        const hub = new HubConnectionBuilder()
          .withUrl(url, {
            withCredentials: false,
            accessTokenFactory: () => accessToken!,
          })
          .withAutomaticReconnect()
          .configureLogging(LogLevel.None)
          .build();

        events?.forEach((e, i) => {
          hub.on(e.methodName, e.newMethod);
        });

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
  }, [url, hubConnection]);

  return { hubState: state, hubConnection };
}

export default useSignalR;
