import { Component, OnInit } from '@angular/core';
import { RouterOutlet, ActivatedRoute, RouterModule, Router, NavigationEnd } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { AvatarImageComponent } from './components/avatar-image/avatar-image.component';
import { MatButtonModule, MatIconButton } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { AuthService } from './services/auth/auth.service';
import { ProgressSpinerComponent } from './components/animations/progress-spiner/progress-spiner.component';
import { CommonModule } from '@angular/common';
import { debounceTime, distinctUntilChanged, map, Observable, OperatorFunction } from 'rxjs';
import { MatMenuTrigger } from '@angular/material/menu';
import { ChatComponent } from './components/chat/chat.component';
import { ShoppingCartComponent } from "./components/shopping-cart/shopping-cart.component";
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';

const states = [
	'Alabama',
	'Alaska',
	'American Samoa',
	'Arizona',
	'Arkansas',
	'California',
	'Colorado',
	'Connecticut',
	'Delaware',
	'District Of Columbia',
	'Federated States Of Micronesia',
	'Florida',
	'Georgia',
	'Guam',
	'Hawaii',
	'Idaho',
	'Illinois',
	'Indiana',
	'Iowa',
	'Kansas',
	'Kentucky',
	'Louisiana',
	'Maine',
	'Marshall Islands',
	'Maryland',
	'Massachusetts',
	'Michigan',
	'Minnesota',
	'Mississippi',
	'Missouri',
	'Montana',
	'Nebraska',
	'Nevada',
	'New Hampshire',
	'New Jersey',
	'New Mexico',
	'New York',
	'North Carolina',
	'North Dakota',
	'Northern Mariana Islands',
	'Ohio',
	'Oklahoma',
	'Oregon',
	'Palau',
	'Pennsylvania',
	'Puerto Rico',
	'Rhode Island',
	'South Carolina',
	'South Dakota',
	'Tennessee',
	'Texas',
	'Utah',
	'Vermont',
	'Virgin Islands',
	'Virginia',
	'Washington',
	'West Virginia',
	'Wisconsin',
	'Wyoming',
];

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule, CommonModule,
    HomeComponent, AvatarImageComponent, ProgressSpinerComponent, ChatComponent,
    MatButtonModule, MatIconModule, MatToolbarModule, MatIconModule, MatButtonModule, MatMenuTrigger, ShoppingCartComponent,
	NgbTypeaheadModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  image: string = 'brand-logo';
  link: string = 'link';
  right_toolbar_items: boolean = true;
  search_item: boolean = true;
  isProfile$?: Observable<boolean>;
  model: any;

  constructor(private router: Router, 
    private auth: AuthService
  ){}
  ngOnInit(): void {
    
    this.auth.setAuthStatus()
    this.isProfile$ = this.auth.authStatus;

    this.router.events.subscribe(event =>{
      if(event instanceof NavigationEnd){
        const url = event.urlAfterRedirects;
        if(!url.includes('/login') && !url.includes('/signup')){
          this.image = 'brand-logo-home';
          this.link = 'link-home';
          this.right_toolbar_items = true;
		  this.search_item = true;
        }
        else{
          setTimeout(() => {
            this.image = 'brand-logo-home'
            this.link = 'link-home'
          },1000)
          this.right_toolbar_items = false;
		  this.search_item = false;
        }
      }
    })

  }

  search: OperatorFunction<string, readonly string[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map((term) =>
        term.length < 2 ? [] : states.filter((v) => v.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10),
      ),
    );
}
