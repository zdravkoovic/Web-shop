import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { ImageService } from '../../services/image.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth/auth.service';
import { Router, NavigationEnd } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { BehaviorSubject, Observable } from 'rxjs';
import { ProfilePictureService } from '../../services/profile-picture.service';

@Component({
  selector: 'app-avatar-image',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatMenuModule],
  templateUrl: './avatar-image.component.html',
  styleUrl: './avatar-image.component.scss'
})
export class AvatarImageComponent implements OnInit{
  showAvatar: boolean = true;
  path?: Observable<SafeUrl>
  firstName!: string;
  lastName!: string;

  constructor(private image: ImageService,
        private profilePictureService : ProfilePictureService,
        private auth: AuthService,
        private router: Router
      ){
  }
  ngOnInit(): void {
    this.router.events.subscribe(event => {
      if(event instanceof NavigationEnd){
        let url = event.urlAfterRedirects;
        this.showAvatar = !(url.includes('/login') || url.includes('/signup'));
        if(this.auth.isLoggedIn()) this.fetchProfilePicture();
      }
    })
  
    this.firstName = this.auth.getFirstName()!;
    this.lastName = this.auth.getLastName()!;
    this.path = this.fetchProfilePicture();
    this.path.subscribe(r => {
      this.profilePictureService.updatePicture(r);
    })
  }

  logout() : void {
    this.auth.logout();
  }

  fetchProfilePicture(): Observable<SafeUrl> {
    return this.image.downloadAvatar()
  }
  
}
