import { Component, Directive } from '@angular/core';

@Component({
  imports: [],
  selector: 'lib-options-menu',
  styleUrl: './options-menu.component.scss',
  templateUrl: './options-menu.component.html',
})
export class OptionsMenuComponent {}

@Directive({
  selector: 'lib-options-menu-header, [libOptionsMenuHeader]'
})
export class OptionsMenuHeaderDirective {}

@Directive({
  selector: 'lib-options-menu-content, [libOptionsMenuContent]'
})
export class OptionsMenuContentDirective {}
