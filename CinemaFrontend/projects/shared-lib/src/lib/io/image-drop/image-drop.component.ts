import { Component, forwardRef, input, signal, ViewChild } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  imports: [],
  selector: 'lib-image-drop',
  styleUrl: './image-drop.component.scss',
  templateUrl: './image-drop.component.html',
    providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ImageDropComponent),
      multi: true
    }
  ]
})
export class ImageDropComponent {
  inputId = input.required<string>();
  placeholder = input<string>();

  isActive = false;
  formats = ['image/png', 'image/jpeg'];
  imagePreviewUrl = signal<string | null>(null);

  value : File | null = null;
  disabled = false;

  private onChange: (value: File | null) => void = () => {};
  private onTouched: () => void = () => {};

  onEnterArea() {
    this.isActive = true;
  }

  onLeaveArea() {
    this.isActive = false;
  }

  writeValue(value: File | null): void {
    this.value = value ?? null;
  }

  registerOnChange(fn: (value: File | null) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  handleInput(event: Event): void {
    console.log('handleInput', event);
    let value = (event.target as HTMLInputElement).files?.[0] || null;
    if (value && !this.formats.includes(value.type)) {
      value = null;
      this.imagePreviewUrl.set(null);
    }
    else if (value) {
      this.imagePreviewUrl.set(URL.createObjectURL(value));
    }
    else {
      this.imagePreviewUrl.set(null);
    }

    this.value = value;
    this.onChange(value);
  }

  handleBlur(): void {
    this.onTouched();
  }
}
