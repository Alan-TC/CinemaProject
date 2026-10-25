import { Router, RouterOutlet } from '@angular/router';
import { OptionsSelectorComponent } from './../../../../../shared-lib/src/lib/pages/options/options-selector/options-selector.component';
import { Component, inject } from '@angular/core';
import { AuthService } from '../../core/auth/services/auth.service';

@Component({
  imports: [OptionsSelectorComponent, RouterOutlet],
  selector: 'cln-my-account',
  styleUrl: './my-account.component.scss',
  templateUrl: './my-account.component.html',
})
export class MyAccountComponent {
  private readonly _authService = inject(AuthService);
  private readonly _router = inject(Router);

  optionsItems = [
    { label: 'Perfil', route: 'profile' },
    { label: 'Ingressos', route: '/tickets' },
    { label: 'Sair', action: 'logout' }
  ];

  onOptionsItemsActionHandler(action: string) {
    if (action === 'logout') {
      this._authService.clear_tokens();
      this._router.navigate(['/']);
    }
  }
}
