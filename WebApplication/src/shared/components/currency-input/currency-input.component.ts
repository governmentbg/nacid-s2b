import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, ViewChild, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NgModel } from "@angular/forms";

@Component({
    selector: 'currency-input',
    templateUrl: './currency-input.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => CurrencyInputComponent),
        multi: true
    }]
})
export class CurrencyInputComponent implements ControlValueAccessor {

    @Input() model: number = null;
    @Input() formControlClass = 'form-control form-control-sm';
    @Input() inputGroupClass = 'input-group input-group-sm';
    @Input() disabled = false;
    @Input() required = false;
    @Input() placeholder = '0';
    @Input() min = 1;
    @Input() max: number = null;
    @Input() step = 1;
    @Input() appendText: string = null;

    @ViewChild(NgModel) currencyInput: NgModel;

    constructor(
        private changeDetectorRef: ChangeDetectorRef
    ) {
    }

    setValueFromInside(newValue: number) {
        if (this.currencyInput.valid) {
            this.model = newValue;
            this.propagateChange(newValue);
            this.propagateTouched();
        } else {
            this.propagateChange(null);
            this.propagateTouched();
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
        this.model = value;
        this.changeDetectorRef.detectChanges();
    }
}