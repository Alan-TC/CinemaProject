import { Component, input } from '@angular/core';

@Component({
  imports: [],
  selector: 'lib-logo',
  styleUrl: './logo.component.scss',
  templateUrl: './logo.component.html',
})
export class LogoComponent {
  readonly subText = input<string>('');
}
