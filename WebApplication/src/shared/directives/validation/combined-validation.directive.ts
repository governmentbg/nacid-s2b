import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

export function MatchPasswordValidator(passwordKey: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const passwordControl = control.get(passwordKey);
        const confirmPasswordControl = control.get('passwordAgain' || 'newPasswordAgain');

        if (passwordControl && confirmPasswordControl) {
            return passwordControl.value === confirmPasswordControl.value ? null : { passwordsMismatch: true };
        }
        return null;
    };
}

@Directive({
    selector: '[combinedPasswordValidation]',
    providers: [
        {
            provide: NG_VALIDATORS,
            useExisting: forwardRef(() => CombinedPasswordDirective),
            multi: true
        }
    ]
})
export class CombinedPasswordDirective implements Validator {
    @Input('combinedPasswordValidation') passwordKey: string;

    validate(control: AbstractControl): { [key: string]: any } | null {
        const passwordMatchValidation = MatchPasswordValidator(this.passwordKey)(control);
        return passwordMatchValidation;
    }
}
