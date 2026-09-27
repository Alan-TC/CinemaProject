import { Component, inject, OnInit, signal } from '@angular/core';
import { OptionsMenuComponent,  OptionsMenuHeaderDirective, OptionsMenuContentDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { SearchInputComponent } from '../../../../../shared-lib/src/lib/io/search-input/search-input.component';
import { ISessionSummary } from '../../core/sessions/models/session-summary.interface';
import { SessionsService } from '../../core/sessions/services/sessions.service';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  imports: [
    SearchInputComponent,
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective,
    DatePipe
  ],
  selector: 'app-sessions',
  styleUrl: './sessions.component.scss',
  templateUrl: './sessions.component.html',
})
export class SessionsComponent implements OnInit {
  private readonly _sessionsService = inject(SessionsService);
  private readonly _router = inject(Router);

  sessions = signal<ISessionSummary[]>([]);

  currPage = 1;
  pageSize = 10;
  hasNext = true;
  isLoading = false;

  searchCode: string | null = null;

  ngOnInit(): void {
    this.loadMoreSessions();
  }

  onScroll(event: Event): void {
    const element = event.target as HTMLElement;

    const chegouNoFinal =
      element.scrollTop + element.clientHeight >=
      element.scrollHeight - 240;

    if (chegouNoFinal) {
      this.loadMoreSessions();
    }
  }

  onSearchClick(event: string) {
    if (event.trim() === '') {
      this.searchCode = null;
    } else {
      this.searchCode = event.trim();
    }

    this.resetPagination();
  }

  onSessionClick(id: string) {
    this._router.navigate(['/sessions', id]);
  }

  onCreateSessionClick() {
    this._router.navigate(['/sessions/create']);
  }

  resetPagination() {
    this.currPage = 1;
    this.hasNext = true;
    this.sessions.set([]);
    this.loadMoreSessions();
  }

  loadMoreSessions(): void {
    if (this.isLoading || !this.hasNext) {
      return;
    }
    this.isLoading = true;

    this._sessionsService.getSessions(this.currPage, this.pageSize, this.searchCode)
      .subscribe({
        next: (response) => {
          this.currPage++;

          this.hasNext = response.hasNext;
          this.sessions.update((curr) => curr.concat(response.items));
          this.isLoading = false;
        },
        error: (_) => {
          this.isLoading = false;
        }
      });
  }
}
