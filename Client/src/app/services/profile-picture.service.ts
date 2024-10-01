import { Injectable } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProfilePictureService {

  private pictureSubject: BehaviorSubject<SafeUrl> = new BehaviorSubject<SafeUrl>("");
  picture: Observable<SafeUrl> = this.pictureSubject.asObservable();

  constructor() { }

  updatePicture(picture: SafeUrl){
    this.pictureSubject.next(picture);
  }
}
