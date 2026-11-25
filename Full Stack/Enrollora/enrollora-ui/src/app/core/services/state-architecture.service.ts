import { Injectable } from '@angular/core';

export interface StateArchitectureGuidance {
  pattern: 'Service-Based State' | 'NgRx Store';
  recommendedScenario: string;
  benefits: string[];
  tradeoffs: string[];
}

@Injectable({
  providedIn: 'root'
})
export class StateArchitectureService {
  /**
   * Guidance on when to use lightweight service-based state versus NgRx
   */
  getStatePatterns(): StateArchitectureGuidance[] {
    return [
      {
        pattern: 'Service-Based State',
        recommendedScenario: 'Small to medium features, single-domain flows, local state sharing across sibling components.',
        benefits: [
          'Zero boilerplate (no actions, reducers, or effect classes required).',
          'Leverages native RxJS BehaviorSubject or Angular signals directly.',
          'Easy to test and trace within a single service.',
          'Minimal bundle size impact.'
        ],
        tradeoffs: [
          'State mutations are not strictly centralized through pure reducer functions.',
          'No out-of-the-box Redux DevTools time-travel debugging.'
        ]
      },
      {
        pattern: 'NgRx Store',
        recommendedScenario: 'Large enterprise apps with complex cross-feature synchronization, offline persistence, undo/redo, or multi-role real-time workflows.',
        benefits: [
          'Strict unidirectional data flow with immutable state snapshots.',
          'Centralized effects handling for side-effect isolation.',
          'Full Redux DevTools replayability and audit logging.'
        ],
        tradeoffs: [
          'Significant boilerplate (Actions, Reducers, Selectors, Effects).',
          'Steeper learning curve and higher maintenance overhead for simple CRUD apps.'
        ]
      }
    ];
  }
}
