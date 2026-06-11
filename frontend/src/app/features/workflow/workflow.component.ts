import { DecimalPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import {
  BatchFormula,
  Customer,
  Dashboard,
  Dispatch,
  Efficiency,
  Grn,
  Invoice,
  Machine,
  ProductionBatch,
  RawMaterialStock,
  Scrap,
  ScrewBarrel,
  Supplier
} from '../../core/api.models';
import { ApiService } from '../../core/api.service';

type StepStatus = 'pending' | 'running' | 'done' | 'error';

interface FlowStep {
  key: string;
  title: string;
  status: StepStatus;
  result?: string;
}

@Component({
  selector: 'app-workflow',
  standalone: true,
  imports: [DecimalPipe],
  templateUrl: './workflow.component.html',
  styleUrl: './workflow.component.css'
})
export class WorkflowComponent {
  private readonly api = inject(ApiService);
  private readonly today = new Date().toISOString().slice(0, 10);

  running = false;
  error = '';
  materials: RawMaterialStock[] = [];
  supplier?: Supplier;
  grn?: Grn;
  formula?: BatchFormula;
  machine?: Machine;
  screwBarrel?: ScrewBarrel;
  batch?: ProductionBatch;
  scrap?: Scrap;
  customer?: Customer;
  dispatch?: Dispatch;
  invoice?: Invoice;
  stock: RawMaterialStock[] = [];
  efficiency: Efficiency[] = [];
  dashboard?: Dashboard;

  steps: FlowStep[] = [
    { key: 'materials', title: 'Raw material master', status: 'pending' },
    { key: 'grn', title: 'Supplier and GRN stock entry', status: 'pending' },
    { key: 'formula', title: 'Batch formula', status: 'pending' },
    { key: 'production', title: 'Pipe production and socketing', status: 'pending' },
    { key: 'scrap', title: 'Stage-wise scrap reuse', status: 'pending' },
    { key: 'dispatch', title: 'Dispatch challan', status: 'pending' },
    { key: 'invoice', title: 'Invoice linkage', status: 'pending' },
    { key: 'reports', title: 'Reports refresh', status: 'pending' }
  ];

  async runFullFlow(): Promise<void> {
    this.reset();
    this.running = true;
    const suffix = this.makeSuffix();

    try {
      await this.runStep('materials', async () => {
        const calcium = await firstValueFrom(this.api.createRawMaterial({ name: `Calcium Flow ${suffix}`, unit: 'Kg', reorderLevel: 100 }));
        const resin = await firstValueFrom(this.api.createRawMaterial({ name: `Resin Flow ${suffix}`, unit: 'Kg', reorderLevel: 50 }));
        const pulverizer = await firstValueFrom(this.api.createRawMaterial({ name: `Pulverizer Flow ${suffix}`, unit: 'Kg', reorderLevel: 40 }));
        this.materials = [calcium, resin, pulverizer];
        return `${this.materials.length} materials created`;
      });

      await this.runStep('grn', async () => {
        this.supplier = await firstValueFrom(this.api.createSupplier({
          name: `Flow Supplier ${suffix}`,
          country: 'India',
          gstNumber: `GST${suffix}`,
          phone: '9999999999'
        }));

        this.grn = await firstValueFrom(this.api.createGrn({
          supplierId: this.supplier.id,
          receiptDate: this.today,
          qualityStatus: 2,
          items: [
            { rawMaterialId: this.materials[0].id, batchNumber: `CA-${suffix}`, quantityKg: 500 },
            { rawMaterialId: this.materials[1].id, batchNumber: `RE-${suffix}`, quantityKg: 300 },
            { rawMaterialId: this.materials[2].id, batchNumber: `PU-${suffix}`, quantityKg: 200 }
          ]
        }));
        return this.grn.grnNumber;
      });

      await this.runStep('formula', async () => {
        this.formula = await firstValueFrom(this.api.createFormula({
          formulaCode: `PVC-${suffix}`,
          description: 'PVC pipe standard flow formula',
          items: [
            { rawMaterialId: this.materials[0].id, standardQuantityKg: 70 },
            { rawMaterialId: this.materials[1].id, standardQuantityKg: 35 },
            { rawMaterialId: this.materials[2].id, standardQuantityKg: 20 }
          ]
        }));
        return `${this.formula.formulaCode} with ${this.formula.materialCount} items`;
      });

      await this.runStep('production', async () => {
        this.machine = await firstValueFrom(this.api.createMachine({ machineNumber: `M-${suffix}`, description: 'Extruder line' }));
        this.screwBarrel = await firstValueFrom(this.api.createScrewBarrel({ barrelNumber: `SB-${suffix}`, type: 'Standard', targetKgPerHour: 80 }));
        this.batch = await firstValueFrom(this.api.createProductionBatch({
          batchFormulaId: this.formula!.id,
          productionDate: this.today,
          shift: 'A',
          operatorName: 'Flow Operator',
          supervisorName: 'Flow Supervisor',
          consumptions: [
            { rawMaterialId: this.materials[0].id, standardQuantityKg: 70, actualQuantityKg: 72 },
            { rawMaterialId: this.materials[1].id, standardQuantityKg: 35, actualQuantityKg: 34 },
            { rawMaterialId: this.materials[2].id, standardQuantityKg: 20, actualQuantityKg: 19 }
          ],
          outputs: [
            { machineId: this.machine.id, screwBarrelId: this.screwBarrel.id, stage: 1, pipeDimension: '63mm Pipe', approvedQuantityKg: 100, rejectedQuantityKg: 8, hoursRun: 2 },
            { machineId: this.machine.id, screwBarrelId: this.screwBarrel.id, stage: 2, pipeDimension: '63mm Socket', approvedQuantityKg: 95, rejectedQuantityKg: 5, hoursRun: 1 }
          ]
        }));
        return `${this.batch.batchNumber}: ${this.batch.approvedKg} kg approved`;
      });

      await this.runStep('scrap', async () => {
        this.scrap = await firstValueFrom(this.api.recordScrap({
          stage: 1,
          scrapType: 'Damaged Pipes',
          generatedKg: 13,
          reusedKg: 9,
          sourceReference: this.batch!.batchNumber,
          recordDate: this.today
        }));
        return `${this.scrap.generatedKg} kg generated, ${this.scrap.reusedKg} kg reused`;
      });

      await this.runStep('dispatch', async () => {
        this.customer = await firstValueFrom(this.api.createCustomer({
          name: `Flow Customer ${suffix}`,
          gstNumber: `CGST${suffix}`,
          phone: '8888888888',
          address: 'Flow test address'
        }));
        this.dispatch = await firstValueFrom(this.api.createDispatch({
          customerId: this.customer.id,
          dispatchDate: this.today,
          vehicleNumber: `GJ-${suffix.slice(-4)}`,
          items: [{ pipeDimension: '63mm Pipe', quantityKg: 80, quantityPieces: 100 }]
        }));
        return this.dispatch.challanNumber;
      });

      await this.runStep('invoice', async () => {
        this.invoice = await firstValueFrom(this.api.createInvoice({
          dispatchChallanId: this.dispatch!.id,
          invoiceDate: this.today,
          taxableAmount: 10000,
          gstAmount: 1800
        }));
        return `${this.invoice.invoiceNumber}: Rs ${this.invoice.totalAmount}`;
      });

      await this.runStep('reports', async () => {
        await this.refreshReports();
        return 'Stock, efficiency, and dashboard refreshed';
      });
    } catch (error) {
      this.error = error instanceof Error ? error.message : 'Flow failed. Check API and console logs.';
    } finally {
      this.running = false;
    }
  }

  async refreshReports(): Promise<void> {
    const [stock, efficiency, dashboard] = await Promise.all([
      firstValueFrom(this.api.getStock()),
      firstValueFrom(this.api.getEfficiency()),
      firstValueFrom(this.api.getDashboard())
    ]);
    this.stock = stock;
    this.efficiency = efficiency;
    this.dashboard = dashboard;
  }

  private async runStep(key: string, action: () => Promise<string>): Promise<void> {
    const step = this.steps.find(item => item.key === key);
    if (!step) return;
    step.status = 'running';
    try {
      step.result = await action();
      step.status = 'done';
    } catch (error) {
      step.status = 'error';
      step.result = error instanceof Error ? error.message : 'Failed';
      throw error;
    }
  }

  private reset(): void {
    this.error = '';
    this.materials = [];
    this.supplier = undefined;
    this.grn = undefined;
    this.formula = undefined;
    this.machine = undefined;
    this.screwBarrel = undefined;
    this.batch = undefined;
    this.scrap = undefined;
    this.customer = undefined;
    this.dispatch = undefined;
    this.invoice = undefined;
    this.stock = [];
    this.efficiency = [];
    this.dashboard = undefined;
    this.steps = this.steps.map(step => ({ key: step.key, title: step.title, status: 'pending' }));
  }

  private makeSuffix(): string {
    return new Date().toISOString().replace(/\D/g, '').slice(0, 14);
  }
}
