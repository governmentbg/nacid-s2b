import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

export function ConfirmValidator(confirmValidation?: string | number): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        return control.value === confirmValidation ? null : { email: 'not valid email' };
    };
}

@Directive({
    selector: '[confirmValidation]',
    providers: [
        {
            provide: NG_VALIDATORS,
            useExisting: forwardRef(() => ConfirmDirective),
            multi: true
        }
    ]
})


export class ConfirmDirective implements Validator {

    confirmValidation: string | number;
    @Input('confirmValidation')
    set confirmValidationSetter(confirmValidation: string | number) {
        this.confirmValidation = confirmValidation;
    }

    private valFn = ConfirmValidator();

    validate(control: AbstractControl): { [key: string]: any } | null {
        this.valFn = ConfirmValidator(this.confirmValidation);
        return this.valFn(control);
    }
}
