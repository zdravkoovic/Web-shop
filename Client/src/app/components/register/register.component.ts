import { Component, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatButtonModule, MatIconButton } from '@angular/material/button';
import { MatInput } from '@angular/material/input';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterModule, ReactiveFormsModule, CommonModule,
    MatFormField, MatIcon, MatLabel, MatButtonModule, MatInput, MatSuffix, MatIconButton
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  passwordError: string = '';
  hide = signal(true);

  clickEvent(event: MouseEvent){
    this.hide.set(!this.hide());
    event.stopPropagation();
  }

  constructor(private authService: AuthService, private router: Router)
  {}

  public signupForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', Validators.required)
  });

  public onSubmit(){
    if(this.signupForm.valid){
      this.authService.signup({
        username : this.signupForm.value.username!,
        email : this.signupForm.value.email!,
        password : this.signupForm.value.password!
      }).subscribe({
        next: (data: any) => {
          console.log(data)
          this.router.navigate(['/home']);
        },
        error: (err: HttpErrorResponse) => {
          const errObject = JSON.parse(err.error)
          if(errObject && errObject.detail){
            this.passwordError = errObject.detail;
          }
          else{
            this.passwordError = "An unknown error occured...";
          }
        }
      });
    }
  }
}
