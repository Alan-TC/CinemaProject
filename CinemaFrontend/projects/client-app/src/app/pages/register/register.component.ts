import { ButtonComponent } from './../../../../../shared-lib/src/lib/io/button/button.component';
import { InputComponent } from './../../../../../shared-lib/src/lib/io/input/input.component';
import { LogoComponent } from '../../../../../shared-lib/src/lib/brand/logo/logo.component';
import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { UsersService } from '../../core/users/services/users.service';

@Component({
  imports: [
    ButtonComponent,
    LogoComponent,
    InputComponent,
    ReactiveFormsModule,
    RouterModule
  ],
  selector: 'app-register',
  styleUrl: './register.component.scss',
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  registerForm: FormGroup = new FormGroup({
    name: new FormControl('',
      [Validators.required]),
    email: new FormControl('',
      [Validators.required,
      Validators.email]),
    password: new FormControl('',
      [Validators.required]
    ),
    confirmPassword: new FormControl('',
      [Validators.required]
    )
  });

  private readonly _usersService = inject(UsersService);
  private readonly _router = inject(Router);
  private readonly _route = inject(ActivatedRoute);

  onRegister() {
    if (this.registerForm.get('password')?.value !== this.registerForm.get('confirmPassword')?.value) {
      this.registerForm.get('confirmPassword')?.setErrors({ mismatch: true });
    }

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this._usersService.register(this.registerForm.value.name, this.registerForm.value.email, this.registerForm.value.password)
      .subscribe({
        next: (_) => {
          this.returnToLogin();
        },
        error: (error) => {
          if(error.status === 400) {
            this.registerForm.setErrors({ invalidValues: error.error.errorMessages.join(', ') });
          } else if(error.status === 409) {
            this.registerForm.setErrors({ invalidValues: 'E-mail já cadastrado.' });
          }
          else {
            this.registerForm.setErrors({ unknownError: true });
          }
        }
      })
  }

  getReturnUrl() {
    const returnUrl = this._route.snapshot.queryParamMap.get('returnUrl') || '/';
    return returnUrl;
  }

  returnToLogin() {
    this._router.navigate(['/login'], { queryParamsHandling: 'merge' });
  }

  hasInvalidEmailError() {
    return this.registerForm.get('email')?.touched && this.registerForm.get('email')?.hasError('email');
  }

  hasRequiredEmailError() {
    return this.registerForm.get('email')?.touched && this.registerForm.get('email')?.hasError('required');
  }

  hasRequiredNameError() {
    return this.registerForm.get('name')?.touched && this.registerForm.get('name')?.hasError('required');
  }

  hasRequiredConfirmPasswordError() {
    return this.registerForm.get('confirmPassword')?.touched && this.registerForm.get('confirmPassword')?.hasError('required');
  }

  hasPasswordsMismatchError() {
    return this.registerForm.get('confirmPassword')?.touched && this.registerForm.get('confirmPassword')?.value !== this.registerForm.get('password')?.value;
  }

  hasRequiredPasswordError() {
    return this.registerForm.get('password')?.touched && this.registerForm.get('password')?.hasError('required');
  }
}
