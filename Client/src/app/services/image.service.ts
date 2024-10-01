import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Observable, Subject } from 'rxjs';
import { AuthService } from './auth/auth.service';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(
    private http: HttpClient, 
    private sanitizer : DomSanitizer,
    private authService: AuthService
  ) { }

  private getPictureBlob(path: string) : Observable<Blob>
  {
    return this.http.get(environment.baseUrl+"product-image?path="+path, {responseType: "blob"})
  }
  download(path: string) : Observable<SafeUrl>
  {
    var subject = new Subject<SafeUrl>;
    this.getPictureBlob(path).subscribe(res => {
      let url = URL.createObjectURL(res);
      subject.next(this.sanitizer.bypassSecurityTrustUrl(url));
    })
    return subject.asObservable();
  }

  downloadAvatar() : Observable<SafeUrl>
  {
    let header = new HttpHeaders().set(
      'Authorization', 'Bearer ' + this.authService.getToken()
    );

    let subject = new Subject<SafeUrl>;
    this.http.get(environment.baseUrl+"Images/GetAvatar", {headers: header, responseType: "blob"}).subscribe(res=>{
      let url = URL.createObjectURL(res);
      subject.next(this.sanitizer.bypassSecurityTrustUrl(url));
    });
    return subject.asObservable();
  }

}
