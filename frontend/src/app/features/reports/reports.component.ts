import { AsyncPipe, DecimalPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { forkJoin, tap } from 'rxjs';
import { writeFile, utils } from 'xlsx';
import { Dashboard, Efficiency, RawMaterialStock } from '../../core/api.models';
import { ApiService } from '../../core/api.service';

interface ReportPayload {
  dashboard: Dashboard;
  stock: RawMaterialStock[];
  efficiency: Efficiency[];
}

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [AsyncPipe, DecimalPipe],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.css'
})
export class ReportsComponent {
  private readonly api = inject(ApiService);
  reportData: ReportPayload | null = null;
  readonly pageSize = 5;
  stockPage = 1;
  efficiencyPage = 1;

  report$ = forkJoin({
    dashboard: this.api.getDashboard(),
    stock: this.api.getStock(),
    efficiency: this.api.getEfficiency()
  }).pipe(
    tap((report) => {
      this.reportData = report;
    })
  );

  getStockPageCount(length: number): number {
    return Math.max(1, Math.ceil(length / this.pageSize));
  }

  getEfficiencyPageCount(length: number): number {
    return Math.max(1, Math.ceil(length / this.pageSize));
  }

  exportToExcel(): void {
    if (!this.reportData) {
      return;
    }

    const workbook = utils.book_new();
    const summarySheet = utils.aoa_to_sheet([
      ['Metric', 'Value'],
      ['Raw Material Stock (kg)', this.reportData.dashboard.rawMaterialStockKg],
      ['Approved Production (kg)', this.reportData.dashboard.productionApprovedKg],
      ['Rejected Production (kg)', this.reportData.dashboard.productionRejectedKg],
      ['Scrap Generated (kg)', this.reportData.dashboard.scrapGeneratedKg],
      ['Scrap Reused (kg)', this.reportData.dashboard.scrapReusedKg],
      ['Pending Dispatch Count', this.reportData.dashboard.pendingDispatchCount]
    ]);

    const stockSheet = utils.json_to_sheet(
      this.reportData.stock.map((item) => ({
        Material: item.name,
        AvailableKg: item.availableKg,
        ReorderLevel: item.reorderLevel,
        Unit: item.unit
      }))
    );

    const efficiencySheet = utils.json_to_sheet(
      this.reportData.efficiency.map((item) => ({
        Barrel: item.barrelNumber,
        ProducedKg: item.producedKg,
        HoursRun: item.hoursRun,
        KgPerHour: item.kgPerHour,
        TargetKgPerHour: item.targetKgPerHour
      }))
    );

    utils.book_append_sheet(workbook, summarySheet, 'Summary');
    utils.book_append_sheet(workbook, stockSheet, 'Raw Material Stock');
    utils.book_append_sheet(workbook, efficiencySheet, 'Machine Efficiency');

    writeFile(workbook, `reports-${new Date().toISOString().slice(0, 10)}.xlsx`);
  }
}
