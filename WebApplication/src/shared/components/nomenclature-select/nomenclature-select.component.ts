import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, EventEmitter, forwardRef, HostListener, Input, OnDestroy, Output } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";
import { catchError, Observable, Subscription, throwError } from "rxjs";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { FilterDto } from "src/shared/filter-dtos/filter.dto";

@Component({
    selector: 'nomenclature-select',
    templateUrl: './nomenclature-select.component.html',
    styleUrls: ['./nomenclature-select.styles.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => NomenclatureSelectComponent),
        multi: true
    }]
})
export class NomenclatureSelectComponent implements ControlValueAccessor, OnDestroy {

    @Input() restUrl: string = null;
    @Input() placeholder: string = null;
    @Input() textTemplate: string = null;
    @Input() disabled = false;
    @Input() readonly = false;
    @Input() allowClear = true;
    @Input() keyProperty = 'id';
    @Input() limit = 10;
    @Input() includeInactive = false;
    @Input() showSearchBox = true;
    @Input() required = false;
    @Input() formControlClass = 'form-control form-control-sm'
    filter = new FilterDto();
    @Input('filter')
    set filterSetter(filter: any) {
        this.filter = filter;
    }

    @Output() readonly keyPropertyChange = new EventEmitter<number>();

    touched = false;
    options: any[] = [];
    selectedModel: any = null;
    selectOpened = false;
    loading = false;
    totalCount = 0;

    searchSubscription: Subscription = null;

    constructor(
        private elementRef: ElementRef,
        private httpClient: HttpClient,
        private changeDetectorRef: ChangeDetectorRef
    ) {
    }

    @HostListener('document:click', ['$event']) onClickOutside(event: MouseEvent): void {
        if (this.selectOpened
            && !this.elementRef.nativeElement.contains(event.target)
            && (<HTMLTextAreaElement>event.target).id != 'chevronButton') {
            this.closeSelect();
        }
    }

    @HostListener('click', ['$event']) onClick(e?: Event) {
        if (!this.disabled && !this.readonly) {
            if (e && (e.target as Element).className === 'options-item') {
                return;
            }

            this.clickElement();
        }
    }

    @HostListener('keydown', ['$event'])
    keyboardInput(event: KeyboardEvent) {
        if (!this.selectOpened) {
            if (event.key === 'ArrowDown') {
                this.onClick();
            }

            return;
        }

        switch (event.key) {
            case 'Enter':
                this.closeSelect();
                break;
            case 'Escape':
                this.closeSelect();
                break;
        }
    }

    @HostListener('scroll', ['$event'])
    onScroll(event: any) {
        if (!this.loading && this.options && this.options.length < this.totalCount && event.target.offsetHeight + event.target.scrollTop >= event.target.scrollHeight) {
            this.loadOptions();
        }
    }

    clearSelection(event: Event) {
        this.setValueFromInside(null);
        event.stopPropagation();
    }

    optionsChanged(item: any, event: Event) {
        this.setValueFromInside(item);
        this.closeSelect();
        event.stopPropagation();
    }

    textFilterChange(textFilter: string) {
        this.filter.textFilter = textFilter;
        this.filter.offset = 0;
        this.loadOptions();
    }

    private loadOptions() {
        this.unsubscribeSearch();
        this.loading = true;

        this.searchSubscription = this.getFiltered()
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loading = false;
                    this.closeSelect();
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                const currentElements = e.result;
                this.totalCount = e.totalCount;

                if (this.filter.offset) {
                    const tempArray = this.options.slice(0);
                    tempArray.push(...currentElements);
                    this.options = tempArray;
                } else {
                    this.options = currentElements;
                }

                this.filter.offset = this.options.length;
                this.loading = false;
                this.changeDetectorRef.detectChanges();
            });
    }

    private getFiltered(): Observable<SearchResultDto<any>> {
        this.filter.isActive = this.includeInactive ? null : true;
        return this.httpClient.post<SearchResultDto<any>>(`api/${this.restUrl}`, this.filter);
    }

    private clickElement() {
        if (!this.selectOpened) {
            this.filter.limit = this.limit;
            this.filter.offset = 0;
            this.loadOptions();
        }

        this.selectOpened = !this.selectOpened;
    }

    private closeSelect() {
        this.selectOpened = false;
        this.filter.textFilter = null;
        this.options = [];
        this.changeDetectorRef.detectChanges();
    }

    private setValueFromInside(newValue: any) {
        this.selectedModel = newValue;
        this.propagateChange(newValue);
        this.keyPropertyChange.emit(newValue ? newValue[this.keyProperty] : null);
        this.propagateTouched();
        this.touched = true;
    }

    private unsubscribeSearch() {
        if (this.searchSubscription) {
            this.searchSubscription.unsubscribe();
        }
    }

    // ControlValueAccessor implementation start
    propagateChange = (_: any) => { };
    propagateTouched = () => { };
    registerOnChange(fn: (_: any) => void) {
        this.propagateChange = fn;
    }

    registerOnTouched(fn: () => void) {
        this.propagateTouched = fn;
    }

    writeValue(value: any) {
        this.selectedModel = value;
        this.changeDetectorRef.detectChanges();
    }

    ngOnDestroy() {
        this.unsubscribeSearch();
    }
}