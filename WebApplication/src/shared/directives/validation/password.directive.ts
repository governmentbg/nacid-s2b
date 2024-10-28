import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

export function PasswordValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const passwordValidation = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z])[^\u0400-\u04FF]{8,}$/.test(control.value);
        return passwordValidation ? null : { password: 'password is not valid' };
    };
}

@Directive({
    selector: '[passwordValidation]',
    providers: [
        {
            provide: NG_VALIDATORS,
            useExisting: forwardRef(() => PasswordDirective),
            multi: true
        }
    ]
})

export class PasswordDirective implements Validator {
    @Input() enableEmptyValidation = false;

    private valFn = PasswordValidator();
    validate(control: AbstractControl): { [key: string]: any } | null {
        if (!this.enableEmptyValidation) {
            return this.valFn(control);
        } else {
            return control.value ? this.valFn(control) : null;
        }
    }
}
