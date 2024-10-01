import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { LoginResult } from '../../interfaces/login-result';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatButtonModule, MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule,
    MatFormField, MatLabel, MatInput, MatIconButton, MatSuffix, MatIcon, MatButtonModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit 
{
  showText: string = '.welcome not-show-text'
  hide = signal(true);

  clickEvent(event: MouseEvent){
    this.hide.set(!this.hide());
    event.stopPropagation();
  }

  loginResult?: LoginResult;
  protected loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  })

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ){ 
    setTimeout(() => {
      this.showText = 'welcome show-text'
    }, 1500)
  }

  ngOnInit(): void {

  }

  onSubmit() {
    if(this.loginForm.valid){
      this.authService.login(this.loginForm.value)
        .subscribe(() => {
          if(this.authService.isLoggedIn()) {
            this.router.navigate(['/home'])
          }
        });
    }
  }

 
}
