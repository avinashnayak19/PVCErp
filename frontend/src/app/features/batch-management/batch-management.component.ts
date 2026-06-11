import { AsyncPipe, DecimalPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { BehaviorSubject, switchMap } from 'rxjs';
import { ApiService } from '../../core/api.service';
import { BatchHistory, ProductionBatch } from '../../core/api.models';

@Component({
  selector: 'app-batch-management',
  standalone: true,
  imports: [AsyncPipe, DecimalPipe, ReactiveFormsModule],
  templateUrl: './batch-management.component.html',
  styleUrl: './batch-management.component.css'
})
export class BatchManagementComponent {
  private readonly api = inject(ApiService);
  private readonly fb = inject(FormBuilder);
  private readonly refresh$ = new BehaviorSubject<void>(undefined);

  stock$ = this.refresh$.pipe(switchMap(() => this.api.getStock()));
  batchHistory$ = this.refresh$.pipe(switchMap(() => this.api.getBatchHistory()));
  message = '';

  form = this.fb.nonNullable.group({
    calciumId: ['', Validators.required],
    resinId: ['', Validators.required],
    pulverizerId: ['', Validators.required],
    calciumKg: [70, [Validators.required, Validators.min(0)]],
    resinKg: [35, [Validators.required, Validators.min(0)]],
    pulverizerKg: [20, [Validators.required, Validators.min(0)]],
    approvedBy: ['Production Head']
  });

  saveFormula(): void {
    if (this.form.invalid) return;
    const value = this.form.getRawValue();
    const suffix = new Date().toISOString().replace(/\D/g, '').slice(0, 14);
    this.api.createFormula({
      formulaCode: `PVC-${suffix}`,
      description: `PVC formula approved by ${value.approvedBy}`,
      items: [
        { rawMaterialId: value.calciumId, standardQuantityKg: value.calciumKg },
        { rawMaterialId: value.resinId, standardQuantityKg: value.resinKg },
        { rawMaterialId: value.pulverizerId, standardQuantityKg: value.pulverizerKg }
      ]
    }).subscribe(result => {
      this.message = `Formula ${result.formulaCode} saved with ${result.materialCount} materials.`;
      this.refresh$.next();
    });
  }
}
