import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

export function EmailValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        let validEmailRegex = new RegExp('');

        validEmailRegex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        const emailValidation = validEmailRegex.test(control.value?.toLowerCase());
        return emailValidation ? null : { email: 'not valid email' };
    };
}

@Directive({
    selector: '[emailValidation]',
    providers: [
        {
            provide: NG_VALIDATORS,
            useExisting: forwardRef(() => EmailDirective),
            multi: true
        }
    ]
})


export class EmailDirective implements Validator {
    @Input() enableEmptyValidation = false;
    private valFn = EmailValidator();

    validate(control: AbstractControl): { [key: string]: any } | null {
        if (!this.enableEmptyValidation) {
            return this.valFn(control);
        }
        else {
            return control.value ? this.valFn(control) : null;

        }
    }
}
