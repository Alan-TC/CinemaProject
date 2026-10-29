import { Component, ElementRef, ViewChild, effect, input, model, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ITheaterLayout } from './models/theater-layout.interface';

@Component({
  imports: [FormsModule],
  selector: 'cln-theater-layout',
  styleUrl: './theater-layout.component.scss',
  templateUrl: './theater-layout.component.html',
})
export class TheaterLayoutComponent {
  @ViewChild('layoutContainer') layoutContainer!: ElementRef<HTMLDivElement>;
  @ViewChild('layoutElement') layoutElement!: ElementRef<HTMLDivElement>;

  theaterLayout = input.required<ITheaterLayout>();
  errorMessage = input<string>('');
  seatClicked = output<string>();

  widthInitialValue = 0;
  heightInitialValue = 0;

  minZoom = signal<number>(0.1);
  maxZoom = signal<number>(3.0);

  scale = signal<number>(1);
  translateX = 0;
  translateY = 0;

  private hasDragged = false;
  private dragStartX = 0;
  private dragStartY = 0;
  private isDragging = false;
  private startX = 0;
  private startY = 0;
  private pointerCache: PointerEvent[] = [];
  private initialPinchDistance = -1;

  constructor() {
    effect(() => {
      const theaterLayout = this.theaterLayout();


      setTimeout(() => {
        this.fitToScreen();
      }, 0);
    });
  }

  onLayoutClick(event: PointerEvent) {
    if (this.hasDragged) {
      return;
    }

    const target = event.target as HTMLElement;
    if (target instanceof HTMLElement
      && target.classList.contains('layout__cell'))
    {
      if (!target.classList.contains('layout__available')
        && !target.classList.contains('layout__unavailable')
        && !target.classList.contains('layout__selected')) {
        return;
      }

      const seatId = target.dataset['seatid']!;

      this.seatClicked.emit(seatId);
    }
  }

  fitToScreen() {
    if (!this.layoutContainer || !this.layoutElement) return;

    const container = this.layoutContainer.nativeElement;
    const layout = this.layoutElement.nativeElement;

    if (layout.clientWidth === 0 || layout.clientHeight === 0) return;

    const scaleX = container.clientWidth / layout.clientWidth;
    const scaleY = container.clientHeight / layout.clientHeight;

    const fitScale = Math.min(scaleX, scaleY);

    const minZoom = fitScale * 0.9;
    const maxZoom = Math.max(3.0, fitScale * 4);

    if (minZoom == this.minZoom() && maxZoom == this.maxZoom()) {
      return;
    }

    this.minZoom.set(minZoom);
    this.maxZoom.set(maxZoom);

    if (this.minZoom() > this.maxZoom()) {
      this.maxZoom.set(this.minZoom() * 2);
    }

    this.scale.set(this.minZoom());

    this.translateX = 0;
    this.translateY = 0;

    this.clampBounds();
  }

  onWheel(event: WheelEvent) {
    event.preventDefault();
    const zoomIntensity = 0.5;

    if (event.deltaY < 0) {
      this.scale.set(this.scale() + zoomIntensity);
    } else {
      this.scale.set(this.scale() - zoomIntensity);
    }

    this.applyScaleAndClamp()
  }

  onPointerDown(event: PointerEvent) {
    this.pointerCache.push(event);

    if (this.pointerCache.length === 1) {
      this.isDragging = true;
      this.startX = event.clientX;
      this.startY = event.clientY;

      this.dragStartX = event.clientX;
      this.dragStartY = event.clientY;
      this.hasDragged = false;
    } else if (this.pointerCache.length === 2) {
      this.isDragging = false;
      this.initialPinchDistance = this.getDistance(this.pointerCache[0], this.pointerCache[1]);
      this.hasDragged = true;
    }
  }

  onPointerMove(event: PointerEvent) {
    const index = this.pointerCache.findIndex(p => p.pointerId === event.pointerId);
    if (index !== -1) {
      this.pointerCache[index] = event;
    }

    if (this.pointerCache.length === 1 && this.isDragging) {
      const moveX = Math.abs(event.clientX - this.dragStartX);
      const moveY = Math.abs(event.clientY - this.dragStartY);

      if (moveX > 5 || moveY > 5) {
        this.hasDragged = true;
      }

      const dx = event.clientX - this.startX;
      const dy = event.clientY - this.startY;

      this.translateX += dx / this.scale();
      this.translateY += dy / this.scale();

      this.startX = event.clientX;
      this.startY = event.clientY;

      this.clampBounds();
    } else if (this.pointerCache.length === 2) {
      const currentDistance = this.getDistance(this.pointerCache[0], this.pointerCache[1]);

      if (this.initialPinchDistance > 0) {
        const diff = currentDistance - this.initialPinchDistance;
        this.scale.set(this.scale() + diff * 0.005);
        this.applyScaleAndClamp()
      }
      this.initialPinchDistance = currentDistance;
    }
  }

  onPointerUp(event: PointerEvent) {
    this.pointerCache = this.pointerCache.filter(p => p.pointerId !== event.pointerId);

    if (this.pointerCache.length === 1) {
      this.startX = this.pointerCache[0].clientX;
      this.startY = this.pointerCache[0].clientY;
      this.isDragging = true;
    } else {
      this.isDragging = false;
      this.initialPinchDistance = -1;
    }
  }

  private getDistance(p1: PointerEvent, p2: PointerEvent) {
    return Math.hypot(p1.clientX - p2.clientX, p1.clientY - p2.clientY);
  }

  onSliderChange(event: Event) {
    const slider = event.target as HTMLInputElement;
    this.scale.set(parseFloat(slider.value) / 100);
    this.applyScaleAndClamp();
  }

  private applyScaleAndClamp() {
    this.scale.set(Math.max(this.minZoom(), Math.min(this.scale(), this.maxZoom())));

    this.clampBounds();
  }

  private clampBounds() {
    if (!this.layoutContainer || !this.layoutElement) return;

    const container = this.layoutContainer.nativeElement;
    const layout = this.layoutElement.nativeElement;

    const scaledWidth = layout.clientWidth * this.scale();
    const scaledHeight = layout.clientHeight * this.scale();

    const limitX = Math.max(0, (scaledWidth - container.clientWidth) / 2) / this.scale();
    const limitY = Math.max(0, (scaledHeight - container.clientHeight) / 2) / this.scale();

    this.translateX = Math.max(-limitX, Math.min(this.translateX, limitX));
    this.translateY = Math.max(-limitY, Math.min(this.translateY, limitY));
  }
}
