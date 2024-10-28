import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

export function UicValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const isWhitespace = (control.value || '').trim().length === 0;
        if (isWhitespace) {
            return { whitespace: 'value is only whitespace' };
        } else {
            const uicPattern = /^[0-9]\d*$/im;
            return ((control.value?.length === 9 || control.value?.length === 10 || control.value?.length === 13)
                && uicPattern.test(control.value)) ? null : { uic: 'value is not a valid phone number' }
        }
    };
}

@Directive({
    selector: '[uicValidation]',
    providers: [
        {
            provide: NG_VALIDATORS,
            useExisting: forwardRef(() => UicDirective),
            multi: true
        }
    ]
})

export class UicDirective implements Validator {

    @Input() enableEmptyValidation = false;

    private valFn = UicValidator();
    validate(control: AbstractControl): { [key: string]: any } | null {
        if (!this.enableEmptyValidation) {
            return this.valFn(control);
        } else {
            return control.value ? this.valFn(control) : null;
        }
    }
}
