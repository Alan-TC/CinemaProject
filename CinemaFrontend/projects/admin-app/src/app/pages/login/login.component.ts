import { ButtonComponent } from '../../../../../shared-lib/src/lib/io/button/button.component';
import { LogoComponent } from '../../../../../shared-lib/src/lib/brand/logo/logo.component';
import { Component } from '@angular/core';

@Component({
  imports: [ ButtonComponent, LogoComponent ],
  selector: 'app-login',
  styleUrl: './login.component.scss',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  teste() {
    console.log('teste');
  }
}
