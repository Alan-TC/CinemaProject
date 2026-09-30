import { Component, inject, signal } from '@angular/core';
import { TheatersLayoutService } from '../../core/theaters-layout/services/theaters-layout.service';
import { Router } from '@angular/router';
import { ITheaterLayout } from '../../core/theaters-layout/models/theater-layout.interface';
import { SearchInputComponent } from '../../../../../shared-lib/src/lib/io/search-input/search-input.component';
import { OptionsMenuComponent, OptionsMenuContentDirective, OptionsMenuHeaderDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { DatePipe } from '@angular/common';
import { LayoutPreviewComponent } from './components/layout-preview/layout-preview.component';

@Component({
  imports: [
    LayoutPreviewComponent,
    SearchInputComponent,
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective,
    DatePipe
  ],
  selector: 'app-theaters-layout',
  styleUrl: './theaters-layout.component.scss',
  templateUrl: './theaters-layout.component.html',
})
export class TheatersLayoutComponent {
  private readonly _theatersLayoutService = inject(TheatersLayoutService);
  private readonly _router = inject(Router);

  theatersLayout = signal<ITheaterLayout[]>([]);

  currPage = 1;
  pageSize = 10;
  hasNext = true;
  isLoading = false;

  searchCode: string | null = null;

  ngOnInit(): void {
    this.loadMoreTheatersLayout();
  }

  onScroll(event: Event): void {
    const element = event.target as HTMLElement;

    const chegouNoFinal =
      element.scrollTop + element.clientHeight >=
      element.scrollHeight - 240;

    if (chegouNoFinal) {
      this.loadMoreTheatersLayout();
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

  onTheatersLayoutClick(id: string) {
    this._router.navigate(['/theaters-layout', id]);
  }

  onCreateTheatersLayoutClick() {
    this._router.navigate(['/theaters-layout/create']);
  }

  resetPagination() {
    this.currPage = 1;
    this.hasNext = true;
    this.theatersLayout.set([]);
    this.loadMoreTheatersLayout();
  }

  loadMoreTheatersLayout(): void {
    if (this.isLoading || !this.hasNext) {
      return;
    }
    this.isLoading = true;

    this._theatersLayoutService.getTheatersLayout(this.currPage, this.pageSize, this.searchCode)
      .subscribe({
        next: (response) => {
          this.currPage++;
          this.hasNext = response.hasNext;

          this.theatersLayout.update((curr) => curr.concat(response.items));
          this.isLoading = false;
        },
        error: (_) => {
          this.isLoading = false;
        }
      });
  }
}
