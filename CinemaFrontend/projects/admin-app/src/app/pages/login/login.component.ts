import { ButtonComponent } from '../../../../../shared-lib/src/lib/io/button/button.component';
import { InputComponent } from '../../../../../shared-lib/src/lib/io/input/input.component';
import { LogoComponent } from '../../../../../shared-lib/src/lib/brand/logo/logo.component';
import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../core/auth/services/auth.service';
import { Router } from '@angular/router';

@Component({
  imports: [ ButtonComponent, LogoComponent, InputComponent, ReactiveFormsModule ],
  selector: 'app-login',
  styleUrl: './login.component.scss',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  loginForm: FormGroup = new FormGroup({
    email: new FormControl('',
      [Validators.required,
      Validators.email]),
    password: new FormControl('',
      [Validators.required]
    )
  });

  private readonly _authService = inject(AuthService);
  private readonly _router = inject(Router);

  onLogin() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this._authService.login(this.loginForm.value.email, this.loginForm.value.password)
      .subscribe({
        next: (_) => {
          this._router.navigate(['/']);
        },
        error: (error) => {
          if(error.status === 401) {
            this.loginForm.setErrors({ invalidCredentials: true });
          } else {
            this.loginForm.setErrors({ unknownError: true });
          }
        }
      })
  }

  hasInvalidEmailError() {
    return this.loginForm.get('email')?.touched && this.loginForm.get('email')?.hasError('email');
  }

  hasRequiredEmailError() {
    return this.loginForm.get('email')?.touched && this.loginForm.get('email')?.hasError('required');
  }

  hasRequiredPasswordError() {
    return this.loginForm.get('password')?.touched && this.loginForm.get('password')?.hasError('required');
  }
}
