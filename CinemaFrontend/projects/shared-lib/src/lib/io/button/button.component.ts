import { Component, input } from '@angular/core';

@Component({
  imports: [],
  selector: 'lib-button',
  styleUrl: './button.component.scss',
  templateUrl: './button.component.html',
})
export class ButtonComponent {
  readonly name = input<string>('Button');
  readonly type = input<'button' | 'submit' | 'reset'>('button');
  readonly styleType = input<'confirm' | 'negative' | 'neutral' >('confirm');
}
