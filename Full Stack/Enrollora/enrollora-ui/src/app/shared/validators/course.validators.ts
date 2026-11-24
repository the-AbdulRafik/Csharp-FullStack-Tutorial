import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function positiveNumberValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (value === null || value === undefined || value === '') {
      return null;
    }
    const num = Number(value);
    if (isNaN(num) || num <= 0) {
      return { positiveNumber: { value: control.value } };
    }
    return null;
  };
}

export function courseNameValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value as string;
    if (!value) {
      return null;
    }
    if (value.trim().length < 3) {
      return { minLengthTrimmed: { requiredLength: 3, actualLength: value.trim().length } };
    }
    return null;
  };
}
