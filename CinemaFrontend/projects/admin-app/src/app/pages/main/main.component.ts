import { Component, inject } from '@angular/core';
import { OptionsComponent } from './options/component/options.component';
import { HeaderComponent } from '../../../../../shared-lib/src/lib/io/header/header.component';
import { AuthService } from '../../core/auth/services/auth.service';
import { Router } from '@angular/router';

@Component({
  imports: [OptionsComponent, HeaderComponent],
  selector: 'app-main',
  styleUrl: './main.component.scss',
  templateUrl: './main.component.html',
})
export class MainComponent {
  private readonly _authService = inject(AuthService);
  private readonly _router = inject(Router);

  profileNavItems = [
    { label: 'Home', route: '/' },
    { label: 'Sair', action: 'logout' }
  ];

  onProfileNavActionHandler(action: string) {
    if (action === 'logout') {
      this.logout();
    }
  }

  private logout() {
    this._authService.clear_tokens();
    this._router.navigate(['/login']);
  }
}
