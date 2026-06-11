
import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BehaviorSubject, switchMap } from 'rxjs';
import { ApiService } from '../../core/api.service';

@Component({
  selector: 'app-socketing',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './socketing.component.html',
  styleUrl: './socketing.component.css'
})
export class SocketingComponent implements OnInit {
  private readonly api = inject(ApiService);
  private readonly fb = inject(FormBuilder);
  private readonly refresh$ = new BehaviorSubject<void>(undefined);

  socketingRecords$ = this.refresh$.pipe(switchMap(() => this.api.getSocketing()));

  socketForm: FormGroup = this.fb.group({
    pipesReceived: [null, [Validators.required, Validators.min(1)]],
    pipesSocketed: [null, [Validators.required, Validators.min(0)]],
    rejectedQty: [null, [Validators.required, Validators.min(0)]],
    scrapWeight: [null, [Validators.required, Validators.min(0)]],
    shift: ['Day', Validators.required],
    entryDate: [new Date().toISOString().substring(0, 10), Validators.required]
  });

  socketMessage = '';

  ngOnInit(): void {
    this.refresh$.next();
  }

  private toNumber(value: unknown): number {
    const parsed = typeof value === 'number' ? value : Number(value);
    return Number.isFinite(parsed) ? parsed : 0;
  }

  getReceivedQty(record: any): number {
    const candidates = [
      record?.pipesReceived,
      record?.PipesReceived,
      record?.receivedQty,
      record?.receivedQuantity,
      record?.pipes_received
    ];
    const value = candidates.find((item) => item !== null && item !== undefined && item !== '');
    return this.toNumber(value);
  }

  getRejectedQty(record: any): number {
    const candidates = [
      record?.rejectedQty,
      record?.RejectedQty,
      record?.RejectedQuantity,
      record?.rejectedQuantity,
      record?.rejected_qty,
      record?.rejectedQtyValue,
      record?.rejectedValue
    ];
    const value = candidates.find((item) => item !== null && item !== undefined && item !== '');
    return this.toNumber(value);
  }

  getScrapWeight(record: any): number {
    const candidates = [record?.scrapWeight, record?.ScrapWeight, record?.scrap_weight];
    const value = candidates.find((item) => item !== null && item !== undefined && item !== '');
    return this.toNumber(value);
  }

  getRejectionPercent(record: any): number {
    const received = this.getReceivedQty(record);
    const rejected = this.getRejectedQty(record);
    return received > 0 ? (rejected / received) * 100 : 0;
  }

  saveSocket(): void {
    if (this.socketForm.invalid) {
      this.socketForm.markAllAsTouched();
      return;
    }

    const value = this.socketForm.getRawValue();
    const rejectedQty = this.toNumber(this.socketForm.get('rejectedQty')?.value);
    const pipesReceived = this.toNumber(value.pipesReceived);
    const pipesSocketed = this.toNumber(value.pipesSocketed);
    const scrapWeight = this.toNumber(value.scrapWeight);

    const payload: any = {
      pipesReceived,
      pipesSocketed,
      rejectedQty,
      scrapWeight,
      shift: value.shift,
      entryDate: value.entryDate,
      createdAtUtc: new Date().toISOString(),
      RejectedQty: rejectedQty,
      RejectedQuantity: rejectedQty,
      rejectedquantity: rejectedQty,
      rejectedQuantity: rejectedQty,
      rejected_qty: rejectedQty,
      PipesReceived: pipesReceived,
      PipesSocketed: pipesSocketed,
      ScrapWeight: scrapWeight,
      Shift: value.shift,
      EntryDate: value.entryDate,
      CreatedAtUtc: new Date().toISOString()
    };

    this.api.createSocketing(payload).subscribe({
      next: (socketing) => {
        this.socketMessage = `Socketing entry ${socketing.id ?? 'saved'} saved.`;
        this.socketForm.reset({ shift: 'Day', entryDate: new Date().toISOString().substring(0, 10) });
        this.refresh$.next();
      },
      error: () => {
        this.socketMessage = 'Failed to save socketing entry.';
      }
    });
  }
}



