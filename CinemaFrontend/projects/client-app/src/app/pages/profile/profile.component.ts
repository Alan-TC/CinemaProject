import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { OptionsMenuComponent, OptionsMenuContentDirective, OptionsMenuHeaderDirective } from './../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { Component, inject } from '@angular/core';
import { UsersService } from '../../core/user/services/users.service';
import { IUserProfile } from '../../core/user/models/user-profile.interface';
import { ButtonComponent } from '../../../../../shared-lib/src/lib/io/button/button.component';

@Component({
  imports: [
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective,
    ReactiveFormsModule,
    ButtonComponent
  ],
  selector: 'cln-profile',
  styleUrl: './profile.component.scss',
  templateUrl: './profile.component.html',
})
export class ProfileComponent {
  private readonly _usersService = inject(UsersService);

  private userProfile: IUserProfile = {
    name: '',
    email: ''
  }

  isEditMode: boolean = false;
  isLoading: boolean = false;

  profileForm = new FormGroup({
    name: new FormControl('',
      [Validators.required]
    ),
    email: new FormControl('',
      [Validators.required,
      Validators.email]),
  })

  ngOnInit() {
    this.formSwitchEditMode(false);
    this.loadUserProfile();
  }

  saveProfile() {
    this.onCancel();
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }
  }
  hasRequiredNameError() {
    return this.profileForm.get('name')?.touched && this.profileForm.get('name')?.hasError('required');
  }

  hasInvalidEmailError() {
    return this.profileForm.get('email')?.touched && this.profileForm.get('email')?.hasError('email');
  }

  hasRequiredEmailError() {
    return this.profileForm.get('email')?.touched && this.profileForm.get('email')?.hasError('required');
  }

  loadUserProfile() {
    if (this.isLoading) {
      return;
    }

    this.isLoading = true;

    this._usersService.getProfile()
      .subscribe((profile) => {
        this.profileForm.patchValue({
          name: profile.name,
          email: profile.email,
        });
        this.userProfile = profile;
        this.isLoading = false;
      });
  }

  onCancel() {
    this.formSwitchEditMode(false);
    this.profileForm.patchValue({
      name: this.userProfile.name,
      email: this.userProfile.email,
    });
  }

  formSwitchEditMode(toEditMode: boolean) {
    if (toEditMode) {
      this.isEditMode = true;
      this.profileForm.get('name')?.enable();
      this.profileForm.get('email')?.enable();
    }
    else {
      this.isEditMode = false;
      this.profileForm.get('name')?.disable();
      this.profileForm.get('email')?.disable();
    }
  }
}
