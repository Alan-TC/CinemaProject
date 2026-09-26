import { Component, input } from '@angular/core';

@Component({
  imports: [],
  selector: 'lib-search-input',
  styleUrl: './search-input.component.scss',
  templateUrl: './search-input.component.html',
})
export class SearchInputComponent {
  readonly type = input<string>('text');
  readonly placeholder = input<string>('');

  value = '';
  disabled = false;

  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  writeValue(value: string | null): void {
    this.value = value ?? '';
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  handleInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value;

    this.value = value;
    this.onChange(value);
  }

  handleBlur(): void {
    this.onTouched();
  }
}
