import { Component, input, output } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  imports: [ RouterLink ],
  selector: 'lib-user',
  styleUrl: './user.component.scss',
  templateUrl: './user.component.html',
})
export class UserComponent {
  profileNav: boolean = false;
  readonly profileNavItems = input<{label: string, route?: string, action?: string}[]>([]);
  readonly action = output<string>();

  toggleProfileNav() {
    if (!this.hasProfileNavItems) return;
    this.profileNav = !this.profileNav;
  }

  get hasProfileNavItems() {
    return this.profileNavItems().length > 0;
  }
}
