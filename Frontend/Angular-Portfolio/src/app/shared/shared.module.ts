import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { CardWrapperComponent } from './components/card-wrapper/card-wrapper.component';
import { SectionHeaderComponent } from './components/section-header/section-header.component';
import { ProjectCardComponent } from './components/project-card/project-card.component';
import { QuickFilterComponent } from './components/quick-filter/quick-filter.component';

import { CardHoverDirective } from './directives/card-hover.directive';
import { FilterProjectsPipe } from './pipes/filter-projects.pipe';
import { TruncatePipe } from './pipes/truncate.pipe';

@NgModule({
  declarations: [
    CardWrapperComponent,
    SectionHeaderComponent,
    ProjectCardComponent,
    QuickFilterComponent,
    CardHoverDirective,
    FilterProjectsPipe,
    TruncatePipe
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule
  ],
  exports: [
    CommonModule,
    FormsModule,
    RouterModule,
    CardWrapperComponent,
    SectionHeaderComponent,
    ProjectCardComponent,
    QuickFilterComponent,
    CardHoverDirective,
    FilterProjectsPipe,
    TruncatePipe
  ]
})
export class SharedModule {}
