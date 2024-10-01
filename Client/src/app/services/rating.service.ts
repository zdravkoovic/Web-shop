import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { CommentRequest } from '../interfaces/http/comment-request';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { AuthService } from './auth/auth.service';
import { environment } from '../../environments/environment.development';
import { Comments } from '../interfaces/comments';

@Injectable({
  providedIn: 'root'
})
export class RatingService {

  private rating: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  rating$: Observable<number> = this.rating.asObservable();

  constructor(
    private http: HttpClient,
    private authService : AuthService
  ) { }

  rate(number: number): void {
    this.rating.next(number);
  }

  leaveYourComment(comment: CommentRequest) : Observable<Comments>{
    var header = new HttpHeaders().set(
      'Authorization', 'Bearer ' + this.authService.getToken()
    );

    return this.http.post<Comments>(environment.baseUrl+"leave-your-comment", comment, {headers: header });
  }
}
