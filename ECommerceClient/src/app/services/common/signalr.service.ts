import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private _connections = new Map<string, { isMethodBinded: Map<string, boolean>, hubConnection: HubConnection }>();

  getConnection(hubKey: string): HubConnection {
    return this._connections.get(hubKey)?.hubConnection;
  }

  start(baseHubUrl: string, hubKey: string) {
    let hubUrl = `http://${baseHubUrl}${hubKey}`;

    let _connection = this.getConnection(hubKey);

    if (!_connection || _connection?.state == HubConnectionState.Disconnected) {
      const builder: HubConnectionBuilder = new HubConnectionBuilder();

      const hubConnection: HubConnection = builder.withUrl(hubUrl).withAutomaticReconnect().build();

      hubConnection.start()
        .then(() => {
          console.log("Connected");
        })
        .catch(error => setTimeout(() => this.start(hubUrl, hubKey), 2000));

      _connection = hubConnection;

      if (this._connections.has(hubKey) == false) {

        this._connections.set(hubKey, { hubConnection: _connection, isMethodBinded: new Map<string, boolean>() });
      }
    }

    _connection.onreconnected(connectionId => console.log("Reconnected"));
    _connection.onreconnecting(error => console.log("Reconnecting"));
    _connection.onclose(error => console.log("Close Reconnection"));
  }

  invoke(methodName: string, hubKey: string, successCallback?: (value) => void, errorCallback?: (error) => void, ...messages: any[]) {

    this.getConnection(hubKey).invoke(methodName, messages)
      .then(successCallback)
      .catch(errorCallback);
  }

  on(hubKey: string, methodName: string, callback: (...message: any[]) => void) {
    let connectionModel = this._connections.get(hubKey);

    if (connectionModel?.isMethodBinded.get(methodName) == null || connectionModel?.isMethodBinded.get(methodName)?.valueOf() == false) {
      connectionModel.isMethodBinded.set(methodName, true);
      connectionModel.hubConnection.on(methodName, callback);
    }
  }
}
