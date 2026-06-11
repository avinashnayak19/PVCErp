import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiService } from '../../core/api.service';

@Component({
  selector: 'app-scrap-management',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './scrap-management.component.html',
  styleUrl: './scrap-management.component.css'
})
export class ScrapManagementComponent {
  private readonly api = inject(ApiService);
  private readonly fb = inject(FormBuilder);
  message = '';

  form = this.fb.nonNullable.group({
    scrapType: ['Damaged Pipes', Validators.required],
    stage: [1, Validators.required],
    generatedKg: [28, [Validators.required, Validators.min(0)]],
    reusedKg: [18, [Validators.required, Validators.min(0)]],
    sourceReference: ['Manual scrap entry'],
    recordDate: [new Date().toISOString().slice(0, 10), Validators.required]
  });

  saveScrap(): void {
    if (this.form.invalid) return;
    this.api.recordScrap(this.form.getRawValue()).subscribe(result => {
      this.message = `Scrap saved. Recovery ${result.recoveryPercent.toFixed(1)}%.`;
    });
  }
}
