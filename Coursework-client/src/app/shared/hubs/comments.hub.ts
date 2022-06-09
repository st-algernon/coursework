import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CreateCommentCommand } from '../interfaces';
import { CommentVm } from '../services/api.service';
import { AuthStorage } from '../services/auth.storage';

@Injectable({ providedIn: 'root' }) 
export class CommentsHub {
    private hubConnection: signalR.HubConnection;
    public comment$ = new Subject<CommentVm>();

    constructor (
        private authStorage: AuthStorage
    ) {
        this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(`${environment.apiHost}/hubs/comments`, { withCredentials: false, accessTokenFactory: () => this.authStorage.accessToken ?? '' })
        .build();

        this.hubConnection.serverTimeoutInMilliseconds = 1000 * 30;
    }
    
    startConnection() {
        this.hubConnection
          .start()
          .then(() => console.log('Connection started'))
          .catch((err: string) => console.log('Error while starting connection: ' + err))
    }

    addReceivedCommentsListener() {
        this.hubConnection.on("Receive", (comment: CommentVm) => {
            this.comment$.next(comment);
        });
    }

    sendComment(request: CreateCommentCommand) {
        this.hubConnection.invoke('Send', request);
    }
}