import { Injectable } from '@angular/core';
import { Comments } from '../interfaces/comments';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CommentResponse } from '../interfaces/http/comment-response';
import { AuthService } from './auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {

  constructor(
    private http: HttpClient,
    private authService : AuthService
  ) { }

  getComments(id: number): Observable<CommentResponse>{
    var header = new HttpHeaders().set(
      'Authorization', 'Bearer ' + this.authService.getToken()
    );
    return this.http.get<CommentResponse>('https://localhost:7211/product/comments/'+id, {headers: header});
  }
}
