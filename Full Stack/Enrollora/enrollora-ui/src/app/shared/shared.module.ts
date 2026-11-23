import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppCardComponent } from './components/card/card.component';
import { AppModalComponent } from './components/modal/modal.component';
import { StatusBadgeComponent } from './components/status-badge/status-badge.component';
import { StatusColorDirective } from './directives/status-color.directive';
import { StatusTextPipe } from './pipes/status-text.pipe';
import { CurrencyInrPipe } from './pipes/currency-inr.pipe';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AppCardComponent,
    AppModalComponent,
    StatusBadgeComponent,
    StatusColorDirective,
    StatusTextPipe,
    CurrencyInrPipe
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AppCardComponent,
    AppModalComponent,
    StatusBadgeComponent,
    StatusColorDirective,
    StatusTextPipe,
    CurrencyInrPipe
  ]
})
export class SharedModule {}
