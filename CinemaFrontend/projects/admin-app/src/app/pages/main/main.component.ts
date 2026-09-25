import { Component, inject, Signal } from '@angular/core';
import { HeaderComponent } from '../../../../../shared-lib/src/lib/io/header/header.component';
import { OptionsSelectorComponent } from '../../../../../shared-lib/src/lib/pages/options/options-selector/options-selector.component';
import { AuthService } from '../../core/auth/services/auth.service';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { filter, map } from 'rxjs';
import { LogoComponent } from '../../../../../shared-lib/src/lib/brand/logo/logo.component';

@Component({
  imports: [OptionsSelectorComponent, LogoComponent, HeaderComponent, RouterOutlet],
  selector: 'app-main',
  styleUrl: './main.component.scss',
  templateUrl: './main.component.html',
})
export class MainComponent {
  private readonly _authService = inject(AuthService);
  private readonly _router = inject(Router);

  currentUrl = toSignal(
    this._router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(event => (event as NavigationEnd).urlAfterRedirects)
    ),
    { initialValue: this._router.url }
  );

  profileNavItems = [
    { label: 'Home', route: '/' },
    { label: 'Sair', action: 'logout' }
  ];

  optionsItems = [
    { label: 'Sessões', route: '/sessions' },
    { label: 'Filmes', route: '/movies' },
    { label: 'Salas', route: '/theaters-layout' },
    { label: 'Sair', action: 'logout' }
  ];

  onProfileNavActionHandler(action: string) {
    if (action === 'logout') {
      this.logout();
    }
  }

  onOptionsItemsActionHandler(action: string) {
    if (action === 'logout') {
      this.logout();
    }
  }

  isRouteActive(url: string, exact = false): boolean {
    const current = this.currentUrl();
    return exact ? current === url : current.startsWith(url)
  }

  private logout() {
    this._authService.clear_tokens();
    this._router.navigate(['/login']);
  }
}
