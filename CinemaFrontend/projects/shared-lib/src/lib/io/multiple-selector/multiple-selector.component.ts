import { Component, forwardRef, input, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  imports: [],
  selector: 'lib-multiple-selector',
  styleUrl: './multiple-selector.component.scss',
  templateUrl: './multiple-selector.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => MultipleSelectorComponent),
      multi: true
    }
  ]
})
export class MultipleSelectorComponent implements ControlValueAccessor {
  readonly placeholder = input<string>('');
  readonly inputId = input<string>('');
  readonly options = input.required<{ id: string, label: string }[]>();

  value = signal<string[]>([]);
  disabled = false;

  private onChange: (value: string[]) => void = () => {};
  private onTouched: () => void = () => {};

  getSelectedLabels(): string {
    const selectedOptions = this.options()
      .filter(option => this.value().includes(option.id))
      .map(option => option.label)
      .join(', ');

    return selectedOptions || this.placeholder();
  }

  onSelectedOption(optionId: string): void {
    if (this.value().includes(optionId)) {
      this.value.update(value => {
        return value.filter(id => id !== optionId);
      });
    } else {
      this.value.update(value => [...value, optionId]);
    }
    this.onChange(this.value());
  }

  writeValue(value: string[] | null): void {
    this.value.update(() => value ?? []);
  }

  registerOnChange(fn: (value: string[]) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  handleBlur(): void {
    this.onTouched();
  }
}
