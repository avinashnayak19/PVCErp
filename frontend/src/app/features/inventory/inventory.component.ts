import { AsyncPipe, DatePipe, DecimalPipe, CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { take } from 'rxjs';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { BehaviorSubject, combineLatest, switchMap, map } from 'rxjs';
import { ApiService } from '../../core/api.service';
import { Grn, RawMaterialStock, Supplier } from '../../core/api.models';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [CommonModule, AsyncPipe, DatePipe, DecimalPipe, ReactiveFormsModule],
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent implements OnInit {
  private readonly api = inject(ApiService);
  private readonly fb = inject(FormBuilder);
  private readonly refresh$ = new BehaviorSubject<void>(undefined);
  private readonly currentPage$ = new BehaviorSubject<number>(1);
  private readonly grnPage$ = new BehaviorSubject<number>(1);
  readonly pageSize = 5;
  readonly grnPageSize = 5;
  
  stock$ = this.refresh$.pipe(switchMap(() => this.api.getStock()));
  grnHistory$ = this.refresh$.pipe(switchMap(() => this.api.getGrns()));
  getGrnstock$ = this.refresh$.pipe(switchMap(() => this.api.getGrnstock()));
  getRawmaterials$ = this.refresh$.pipe(switchMap(() => this.api.getRawmaterials()));
  metrics$ = combineLatest([
    this.stock$,
    this.grnHistory$,
    this.getRawmaterials$
  ]).pipe(
    map(([stocks, grns, rawMaterials]) => {
      const getStatus = (available: number, reorder: number) => {
        if (available <= reorder) return 'Low';
        if (available > reorder * 2) return 'Sufficient';
        return 'OK';
      };
      
      return {
        materials: stocks.map(s => ({
          name: s.name,
          available: s.availableKg,
          reorderLevel: s.reorderLevel,
          status: getStatus(s.availableKg, s.reorderLevel),
          unit: s.unit
        })),
        grnCount: grns.length
      };
    })
  );
  
  paginatedStock$ = combineLatest([
    this.stock$,
    this.currentPage$
  ]).pipe(
    map(([items, page]) => {
      const start = (page - 1) * this.pageSize;
      const end = start + this.pageSize;
      return {
        items: items.slice(start, end),
        totalItems: items.length,
        totalPages: Math.ceil(items.length / this.pageSize),
        currentPage: page
      };
    })
  );

  paginatedGrn$ = combineLatest([
    this.getGrnstock$,
    this.grnPage$
  ]).pipe(
    map(([items, page]) => {
      const start = (page - 1) * this.grnPageSize;
      const end = start + this.grnPageSize;
      return {
        items: items.slice(start, end),
        totalItems: items.length,
        totalPages: Math.max(1, Math.ceil(items.length / this.grnPageSize)),
        currentPage: page
      };
    })
  );
  
  lastSupplier?: Supplier;
  lastMaterial?: RawMaterialStock;
  grnMessage = '';

  form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    unit: ['Kg', Validators.required],
    reorderLevel: [0, [Validators.required, Validators.min(0)]]
  });

  grnForm = this.fb.nonNullable.group({
    materialName: ['Calcium', Validators.required],
    supplierName: ['Shree Traders', Validators.required],
    invoiceWeightKg: [500, [Validators.required, Validators.min(1)]],
    actualWeightKg: [498, [Validators.required, Validators.min(1)]],
    qualityStatus: [2, Validators.required],
    dateReceived: [new Date().toISOString().slice(0, 10), Validators.required]
  });

  ngOnInit(): void {
    // set default materialName to first raw material returned (once)
    this.getRawmaterials$.pipe(take(1)).subscribe(list => {
      if (list && list.length) {
        const current = this.grnForm.getRawValue().materialName;
        if (!current) this.grnForm.controls.materialName.setValue(list[0].name);
      }
    });
  }

  goToPage(page: number): void {
    this.currentPage$.next(page);
  }

  goToGrnPage(page: number): void {
    this.grnPage$.next(page);
  }

  save(): void {
    debugger;
    if (this.form.invalid) return;

    this.api.createRawMaterial(this.form.getRawValue()).subscribe(() => {
      this.form.reset({ name: '', unit: 'Kg', reorderLevel: 0 });
      this.currentPage$.next(1);
      this.refresh$.next();
    });
  }

  saveGrn(): void {
    if (this.grnForm.invalid) return;
    const value = this.grnForm.getRawValue();
    const suffix = new Date().toISOString().replace(/\D/g, '').slice(0, 14);

    this.api.createRawMaterial({
      name: `${value.materialName} ${suffix}`,
      unit: 'Kg',
      reorderLevel: 100
    }).subscribe(material => {
      this.lastMaterial = material;
      this.api.createSupplier({
        name: `${value.supplierName} ${suffix}`,
        country: 'India',
        gstNumber: `GST${suffix}`,
        phone: '9999999999'
      }).subscribe(supplier => {
        this.lastSupplier = supplier;
        this.api.createGrn({
          supplierId: supplier.id,
          receiptDate: value.dateReceived,
          qualityStatus: Number(value.qualityStatus),
          items: [{
            rawMaterialId: material.id,
            batchNumber: `GRN-${suffix}`,
            quantityKg: value.actualWeightKg
          }]
        }).subscribe(grn => {
          this.grnMessage = `${grn.grnNumber} saved for ${material.name}.`;
          this.refresh$.next();
        });
      });
    });
  }
}
