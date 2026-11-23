import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusText',
  standalone: true
})
export class StatusTextPipe implements PipeTransform {
  transform(value: string | number | null | undefined, context: 'student' | 'enrollment' = 'student'): string {
    if (value === null || value === undefined) {
      return 'N/A';
    }

    const str = String(value).toLowerCase();

    if (context === 'student') {
      if (str === '0' || str === 'pending') return 'Pending Approval';
      if (str === '1' || str === 'approved') return 'Approved Student';
      if (str === '2' || str === 'rejected') return 'Registration Rejected';
    }

    if (context === 'enrollment') {
      if (str === '1' || str === 'active') return 'Active Enrollment';
      if (str === '2' || str === 'cancelled') return 'Cancelled';
    }

    return String(value);
  }
}
