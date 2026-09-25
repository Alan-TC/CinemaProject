import { Component, input, output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UserComponent } from './user/user.component';

@Component({
  imports: [ UserComponent, RouterLink ],
  selector: 'lib-header',
  styleUrl: './header.component.scss',
  templateUrl: './header.component.html',
})
export class HeaderComponent {
  readonly logoRedirect = input<string>('');
  readonly centerRedirect = input<string>('');
  readonly subName = input<string>('');

  readonly profileNavItems = input<{label: string, route?: string, action?: string}[]>([]);
  readonly action = output<string>();
}
