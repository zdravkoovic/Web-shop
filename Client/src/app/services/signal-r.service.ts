import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr'
import { environment } from '../../environments/environment.development';
import { CommentResponse } from '../interfaces/http/comment-response';
import { Comments } from '../interfaces/comments';
import { BehaviorSubject, Observable } from 'rxjs';
import { ProfilePictureService } from './profile-picture.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection!: signalR.HubConnection;

  private messageSub: BehaviorSubject<Comments | null> = new BehaviorSubject<Comments | null>(null);
  message: Observable<Comments | null> = this.messageSub.asObservable();
  comment: Comments | undefined;

  constructor(
    private profilePictureService: ProfilePictureService
  ) { 
  }

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl( 'https:localhost:7211/commentHub')
      // .configureLogging(signalR.LogLevel.Debug)
      .build();

    this.hubConnection.onclose(err => console.log("Connection closed: " + err))

    this.hubConnection
      .start()
      // .then(() => console.log("Connection started!"))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  public addListenerForComment = (callback: (message: Comments) => void) => {
    this.hubConnection.on('ShareComment', (message: Comments) => {
      callback(message);  
    })  
  }

  public sendTheComment(comment: Comments){

    console.log("Saljemo serveru!")
    if(this.hubConnection.state === signalR.HubConnectionState.Connected)
      this.hubConnection.invoke('LeaveYourComment', comment)
      .then(() => {
        // this.setProfilePicture(comment);
        // this.messageSub.next(comment);
      })
      .catch(rej =>{
        console.log(rej);
      });
      else{
        console.log('Connection is not established.')
      }
  }

  try(message: string){
    if(this.hubConnection.state === signalR.HubConnectionState.Connected)
      this.hubConnection.invoke('Try', message)
      .then(() => console.log("Try is successful"))
      .catch(rej => console.log(rej));
  }

  mess(){
    this.hubConnection.on('ShareComment', (m: Comments) => {
      this.setProfilePicture(m);
      this.messageSub.next(m);
    })
  }

  endConnection(){
    this.hubConnection.stop();
  }

  setProfilePicture(comment: Comments){
    this.profilePictureService.picture.subscribe(r => {
      comment.avatarUrl = r as string;
    })
  }
}
