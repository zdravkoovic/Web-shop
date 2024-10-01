import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginResult } from '../../interfaces/login-result';
import { environment } from '../../../environments/environment.development';
import { SignupRequest } from '../../interfaces/signup-request';
import { L } from '@angular/cdk/keycodes';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private tokenKey = 'authUser'
  private nickname = 'nickname';
  private firstName = 'firstName';
  private lastName = 'lastName';

  private _authStatus = new BehaviorSubject<boolean>(false);
  public authStatus = this._authStatus.asObservable();

  constructor(
    protected http: HttpClient,
    private router: Router
  ) { }

  signup(data: SignupRequest){
    return this.http.post(environment.baseUrl + 'Account/Register', data, {responseType: "text"});
  }
  login(data: any){
    let url = environment.baseUrl + "Account/Login";
    return this.http.post<LoginResult>(url, data, {responseType: 'json'})
      .pipe(tap((result) => {
        if(result.token != null) {
          localStorage.setItem(this.tokenKey, result.token);
          localStorage.setItem(this.nickname, result.nickname);
          localStorage.setItem(this.firstName, result.firstName);
          localStorage.setItem(this.lastName, result.lastName);
          this.setAuthStatus();
        }
      }));
  }

  logout(){
    localStorage.removeItem(this.tokenKey);
    this.setAuthStatus();
    this.router.navigate(['/login']);
  }

  isLoggedIn() : boolean{
    return this.getToken() !== null;
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getNickname(): string | null {
    return localStorage.getItem(this.nickname);
  }

  getFirstName(): string | null {
    return localStorage.getItem(this.firstName);
  }

  getLastName(): string | null {
    return localStorage.getItem(this.lastName);
  }

  setAuthStatus(): void{
    this._authStatus.next(this.isLoggedIn());
  }

}
