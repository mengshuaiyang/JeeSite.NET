import * as signalR from '@microsoft/signalr'

let connection: signalR.HubConnection | null = null

export function getSignalRConnection(): signalR.HubConnection | null {
  return connection
}

export function connectSignalR(token: string): signalR.HubConnection {
  if (connection?.state === signalR.HubConnectionState.Connected)
    return connection

  connection = new signalR.HubConnectionBuilder()
    .withUrl(`/hubs/notifications`, {
      accessTokenFactory: () => token
    })
    .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
    .build()

  connection.onreconnecting(() => console.warn('[SignalR] Reconnecting...'))
  connection.onreconnected(() => console.info('[SignalR] Reconnected'))
  connection.onclose(() => console.warn('[SignalR] Disconnected'))

  connection.start().catch(err => console.error('[SignalR] Connection error:', err))
  return connection
}

export function disconnectSignalR() {
  if (connection) {
    connection.stop()
    connection = null
  }
}
