import { Injectable, signal } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class SessionsMoviesService {
  searchQuery = signal<string | null>(null);

  private readonly _onScroll = new Subject<{scrollTop: number, clientHeight: number, scrollHeight: number}>();
  onScroll$ = this._onScroll.asObservable();

  emitOnScroll(scrollInfo: {scrollTop: number, clientHeight: number, scrollHeight: number}): void {
    this._onScroll.next(scrollInfo);
  }
}
