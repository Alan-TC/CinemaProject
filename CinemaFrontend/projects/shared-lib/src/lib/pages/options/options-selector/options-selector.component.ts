import { Component, input, output } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  imports: [RouterLink, RouterLinkActive],
  selector: 'lib-options-selector',
  styleUrl: './options-selector.component.scss',
  templateUrl: './options-selector.component.html',
})
export class OptionsSelectorComponent {
  readonly optionsItems = input<{label: string, route?: string, action?: string}[]>([]);
  readonly action = output<string>();
}
