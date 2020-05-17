import { Injectable, Output, EventEmitter } from '@angular/core';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  private hubConnection: signalR.HubConnection;

  @Output() teamCreationSignal = new EventEmitter();
  @Output() teamUpdateSignal = new EventEmitter();
  @Output() playerCreationSignal = new EventEmitter();
  @Output() playerUpdateSignal = new EventEmitter();
  @Output() announcementCreationSignal = new EventEmitter();
  @Output() announcementUpdateSignal = new EventEmitter();
  @Output() matchCreationSignal = new EventEmitter();
  @Output() matchUpdateSignal = new EventEmitter();
  @Output() matchEventCreationSignal = new EventEmitter();
  //@Output() matchEventDeleteSignal = new EventEmitter();
  @Output() matchEndedSignal = new EventEmitter();
  

  constructor() {
    this.buildConnection();
    this.startConnection();
   }

   public buildConnection = () => {
     this.hubConnection = new signalR.HubConnectionBuilder()
     .withUrl('http://localhost:53078/chatHub')
     .build();
   }

   public startConnection = () => {
     this.hubConnection.start()
     .then(() => {
       this.registerSignalEvents();
     })
     .catch(err => {
       console.log("Error while starting connection with hub" + err);

       setTimeout(function() { this.startConnection(); }, 3000);
     })
   }

   private registerSignalEvents() {
     this.hubConnection.on('teamCreated', (data: any) => {
       this.teamCreationSignal.emit(data);
     });
     this.hubConnection.on('teamUpdated', (data: any) => {
      this.teamUpdateSignal.emit(data);
    });
    this.hubConnection.on('playerCreated', (data: any) => {
      this.playerCreationSignal.emit(data);
    });
    this.hubConnection.on('playerUpdated', (data: any) => {
      this.playerUpdateSignal.emit(data);
    });
    this.hubConnection.on('announcementCreated', (data: any) => {
      this.announcementCreationSignal.emit(data);
    });
    this.hubConnection.on('announcementUpdated', (data: any) => {
      this.announcementUpdateSignal.emit(data);
    });
    this.hubConnection.on('matchCreated', (data: any) => {
      this.matchCreationSignal.emit(data);
    });
    this.hubConnection.on('matchUpdated', (data: any) => {
      this.matchUpdateSignal.emit(data);
    });
    this.hubConnection.on('matchEventCreated', (data: any) => {
      this.matchEventCreationSignal.emit(data);
    });
    // this.hubConnection.on('matchEventDeleted', (data: any) => {
    //   this.matchEventDeleteSignal.emit(data);
    // });
    this.hubConnection.on('matchEnded', (data: any) => {
      this.matchEndedSignal.emit(data);
    });
   }
}
