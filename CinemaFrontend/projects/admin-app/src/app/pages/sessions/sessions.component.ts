import { Component } from '@angular/core';
import { OptionsMenuComponent,  OptionsMenuHeaderDirective, OptionsMenuContentDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { SearchInputComponent } from '../../../../../shared-lib/src/lib/io/search-input/search-input.component';

@Component({
  imports: [SearchInputComponent, OptionsMenuComponent, OptionsMenuHeaderDirective, OptionsMenuContentDirective ],
  selector: 'app-sessions',
  styleUrl: './sessions.component.scss',
  templateUrl: './sessions.component.html',
})
export class SessionsComponent {}
