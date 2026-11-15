import { Component, inject } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { PortfolioDataService } from '../../core/services/portfolio-data.service';
import { ContactMessage } from '../../models/portfolio.models';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [SharedModule],
  template: `
    <div class="contact-page container">
      <app-section-header
        eyebrow="Communication"
        title="Get in Touch"
        subtitle="Inquire about AI engineering roles, agentic system design, or backend collaboration."
      ></app-section-header>

      <div class="contact-grid">
        <!-- Contact Information Panel -->
        <div class="info-panel">
          <h3 class="panel-title">Direct Channels</h3>
          <p class="panel-desc">
            Feel free to reach out directly via email or professional profiles.
          </p>

          <div class="contact-items">
            <div class="info-card" appCardHover>
              <span class="info-label">Email Address</span>
              <a [href]="'mailto:' + profile.email" class="info-value email-link">
                {{ profile.email }}
              </a>
            </div>

            <div class="info-card" appCardHover>
              <span class="info-label">Location</span>
              <span class="info-value">{{ profile.location }}</span>
            </div>

            <div class="info-card" appCardHover>
              <span class="info-label">Professional Profiles</span>
              <div class="profile-links">
                <a [href]="profile.github" target="_blank" rel="noopener noreferrer" class="link-item">
                  GitHub &rarr;
                </a>
                <a [href]="profile.linkedin" target="_blank" rel="noopener noreferrer" class="link-item">
                  LinkedIn &rarr;
                </a>
              </div>
            </div>
          </div>
        </div>

        <!-- Demonstration Contact Form with Two-Way Binding [(ngModel)] -->
        <div class="form-panel">
          <h3 class="panel-title">Send a Message</h3>
          <p class="panel-desc">
            Local demonstration form showcasing Angular template-driven validation and two-way binding.
          </p>

          <!-- Feedback Banner after local submit -->
          <div class="feedback-banner" *ngIf="submitStatus === 'success'">
            <div class="feedback-header">
              <span class="text-green">&check; Message Preview Generated Successfully</span>
              <button type="button" class="close-btn" (click)="resetForm(contactForm)">&times;</button>
            </div>
            <p class="feedback-text">
              <strong>Local Demonstration Flow:</strong> No server transmission occurred.
            </p>
            <div class="preview-box">
              <p><strong>From:</strong> {{ lastSubmission?.name }} ({{ lastSubmission?.email }})</p>
              <p><strong>Subject:</strong> {{ lastSubmission?.subject }}</p>
              <p><strong>Message:</strong> {{ lastSubmission?.message }}</p>
            </div>
          </div>

          <form
            #contactForm="ngForm"
            (ngSubmit)="onSubmit(contactForm)"
            novalidate
            class="contact-form"
          >
            <!-- Name Field -->
            <div class="form-group">
              <label for="name" class="form-label">Full Name *</label>
              <input
                type="text"
                id="name"
                name="name"
                class="form-control"
                [(ngModel)]="formData.name"
                #nameModel="ngModel"
                required
                minlength="2"
                placeholder="Your name"
              />
              <span
                class="error-text"
                *ngIf="nameModel.invalid && (nameModel.dirty || nameModel.touched)"
              >
                Name is required (at least 2 characters).
              </span>
            </div>

            <!-- Email Field -->
            <div class="form-group">
              <label for="email" class="form-label">Email Address *</label>
              <input
                type="email"
                id="email"
                name="email"
                class="form-control"
                [(ngModel)]="formData.email"
                #emailModel="ngModel"
                required
                email
                placeholder="name@company.com"
              />
              <span
                class="error-text"
                *ngIf="emailModel.invalid && (emailModel.dirty || emailModel.touched)"
              >
                Please provide a valid email address.
              </span>
            </div>

            <!-- Subject Field (Two-Way Binding Demonstration) -->
            <div class="form-group">
              <label for="subject" class="form-label">Subject / Purpose *</label>
              <input
                type="text"
                id="subject"
                name="subject"
                class="form-control"
                [(ngModel)]="formData.subject"
                #subjectModel="ngModel"
                required
                placeholder="e.g. AI Workflow Collaboration"
              />
              <span
                class="error-text"
                *ngIf="subjectModel.invalid && (subjectModel.dirty || subjectModel.touched)"
              >
                Subject is required.
              </span>
            </div>

            <!-- Urgency Radio Buttons Demonstrating [(ngModel)] with Radio Inputs -->
            <div class="form-group">
              <label class="form-label">Inquiry Priority</label>
              <div class="radio-row">
                <label class="radio-label">
                  <input
                    type="radio"
                    name="urgency"
                    value="normal"
                    [(ngModel)]="formData.urgency"
                  />
                  Normal Priority
                </label>
                <label class="radio-label">
                  <input
                    type="radio"
                    name="urgency"
                    value="high"
                    [(ngModel)]="formData.urgency"
                  />
                  High Priority
                </label>
              </div>
            </div>

            <!-- Message Field -->
            <div class="form-group">
              <label for="message" class="form-label">Message Content *</label>
              <textarea
                id="message"
                name="message"
                rows="4"
                class="form-control textarea"
                [(ngModel)]="formData.message"
                #messageModel="ngModel"
                required
                minlength="10"
                placeholder="Detail your inquiry or project scope..."
              ></textarea>
              <span
                class="error-text"
                *ngIf="messageModel.invalid && (messageModel.dirty || messageModel.touched)"
              >
                Message must be at least 10 characters long.
              </span>
            </div>

            <button
              type="submit"
              class="btn btn-primary submit-btn"
              [disabled]="contactForm.invalid"
            >
              Submit Demonstration Message
            </button>
          </form>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .contact-page {
      padding-top: 3rem;
      padding-bottom: 5rem;
    }

    .contact-grid {
      display: grid;
      grid-template-columns: 0.85fr 1.15fr;
      gap: 3rem;
    }

    .panel-title {
      font-size: 1.25rem;
      font-weight: 700;
      color: var(--text-primary);
      margin-bottom: 0.4rem;
    }

    .panel-desc {
      font-size: 0.9rem;
      color: var(--text-secondary);
      margin-bottom: 2rem;
    }

    .contact-items {
      display: flex;
      flex-direction: column;
      gap: 1.25rem;
    }

    .info-card {
      background-color: var(--bg-card);
      border: 1px solid var(--border-color);
      border-radius: 10px;
      padding: 1.25rem;
      display: flex;
      flex-direction: column;
      gap: 0.35rem;
    }

    .info-label {
      font-size: 0.75rem;
      font-family: var(--font-mono);
      text-transform: uppercase;
      letter-spacing: 0.05em;
      color: var(--text-muted);
    }

    .info-value {
      font-size: 1rem;
      font-weight: 600;
      color: var(--text-primary);
    }

    .email-link:hover {
      color: var(--accent-primary);
    }

    .profile-links {
      display: flex;
      gap: 1.25rem;
      margin-top: 0.25rem;
    }

    .link-item {
      font-size: 0.9rem;
      color: var(--accent-primary);
      text-decoration: underline;
    }

    .contact-form {
      background-color: var(--bg-card);
      border: 1px solid var(--border-color);
      border-radius: 12px;
      padding: 2rem;
      display: flex;
      flex-direction: column;
      gap: 1.35rem;
    }

    .form-group {
      display: flex;
      flex-direction: column;
      gap: 0.4rem;
    }

    .form-label {
      font-size: 0.85rem;
      font-weight: 600;
      color: var(--text-primary);
    }

    .form-control {
      background-color: var(--bg-secondary);
      border: 1px solid var(--border-color);
      border-radius: 8px;
      padding: 0.65rem 0.85rem;
      color: var(--text-primary);
      font-family: inherit;
      font-size: 0.9rem;
      outline: none;
      transition: border-color var(--transition-fast);
    }

    .form-control:focus {
      border-color: var(--border-focus);
    }

    .form-control.ng-invalid.ng-touched {
      border-color: #ef4444;
    }

    .textarea {
      resize: vertical;
    }

    .radio-row {
      display: flex;
      gap: 1.5rem;
      margin-top: 0.2rem;
    }

    .radio-label {
      display: flex;
      align-items: center;
      gap: 0.4rem;
      font-size: 0.85rem;
      color: var(--text-secondary);
      cursor: pointer;
    }

    .error-text {
      font-size: 0.75rem;
      color: #ef4444;
    }

    .submit-btn {
      margin-top: 0.5rem;
      width: 100%;
    }

    .submit-btn:disabled {
      opacity: 0.5;
      cursor: not-allowed;
    }

    .feedback-banner {
      background-color: var(--bg-secondary);
      border: 1px solid var(--accent-secondary);
      border-radius: 8px;
      padding: 1.25rem;
      margin-bottom: 1.5rem;
    }

    .feedback-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      font-weight: 600;
      margin-bottom: 0.5rem;
    }

    .text-green {
      color: var(--accent-secondary);
    }

    .close-btn {
      font-size: 1.25rem;
      color: var(--text-muted);
      line-height: 1;
    }

    .feedback-text {
      font-size: 0.85rem;
      color: var(--text-secondary);
      margin-bottom: 0.75rem;
    }

    .preview-box {
      background-color: var(--bg-card);
      border: 1px solid var(--border-color);
      padding: 0.75rem;
      border-radius: 6px;
      font-size: 0.8rem;
      display: flex;
      flex-direction: column;
      gap: 0.35rem;
    }

    @media (max-width: 850px) {
      .contact-grid {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class ContactComponent {
  private readonly dataService = inject(PortfolioDataService);
  readonly profile = this.dataService.profile;

  formData: ContactMessage = {
    name: '',
    email: '',
    subject: '',
    message: '',
    urgency: 'normal'
  };

  submitStatus: 'idle' | 'success' = 'idle';
  lastSubmission: ContactMessage | null = null;

  onSubmit(form: any): void {
    if (form.valid) {
      this.lastSubmission = { ...this.formData };
      this.submitStatus = 'success';
    }
  }

  resetForm(form: any): void {
    form.resetForm({
      urgency: 'normal'
    });
    this.formData = {
      name: '',
      email: '',
      subject: '',
      message: '',
      urgency: 'normal'
    };
    this.submitStatus = 'idle';
  }
}
