import { Component, input, OnInit, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';

@Component({
  imports: [FormsModule],
  selector: 'lib-infinite-selector',
  styleUrl: './infinite-selector.component.scss',
  templateUrl: './infinite-selector.component.html',
})
export class InfiniteSelectorComponent implements OnInit {
  displayProperty = input.required<string>();
  auxDisplayProperty = input<string>('');
  idProperty = input.required<string>();

  paginationFunction = input.required<
  (page: number, pageSize: number, searchByDisplayId: string | null)
  => Observable<{hasNext: boolean, itemsOptions: any[]}>>();

  selectedOptionChange = output<any | null>();

  options = signal<any[]>([]);
  displaySearchTerm: string | null = null;

  currPage = 1;
  pageSize = 10;
  hasNext = true;
  isLoading = false;

  ngOnInit(): void {
    this.loadMoreOptions();
  }

  onSelectedOption(option: any) {
    this.selectedOptionChange.emit(option);

    this.displaySearchTerm = option[this.displayProperty()];
  }

  searchBySearchTerm() {
    this.selectedOptionChange.emit(null);
    this.resetPagination();
  }

  resetPagination() {
    this.currPage = 1;
    this.hasNext = true;
    this.options.set([]);
    this.loadMoreOptions();
  }

  onScroll(event: Event): void {
    const element = event.target as HTMLElement;

    const chegouNoFinal =
      element.scrollTop + element.clientHeight >=
      element.scrollHeight - 240;

    if (chegouNoFinal) {
      this.loadMoreOptions();
    }
  }

  loadMoreOptions(): void {
    if (this.isLoading || !this.hasNext) {
      return;
    }
    this.isLoading = true;

    this.paginationFunction()(this.currPage, this.pageSize, this.displaySearchTerm)
      .subscribe({
        next: (response) => {
          this.currPage++;

          this.hasNext = response.hasNext;
          this.options.update((curr) => curr.concat(response.itemsOptions));
          this.isLoading = false;
        },
        error: (_) => {
          this.isLoading = false;
        }
      });
  }
}
