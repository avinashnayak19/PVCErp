import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReportsComponent } from './reports.component';
import * as xlsx from 'xlsx';

describe('ReportsComponent', () => {
  let component: ReportsComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ReportsComponent, HttpClientTestingModule]
    });

    component = TestBed.createComponent(ReportsComponent).componentInstance;
  });

  it('exports summary and report data to an Excel workbook', () => {
    const writeFileSpy = spyOn(xlsx, 'writeFile');

    component.reportData = {
      dashboard: {
        rawMaterialStockKg: 1200,
        productionApprovedKg: 900,
        productionRejectedKg: 100,
        scrapGeneratedKg: 50,
        scrapReusedKg: 25,
        pendingDispatchCount: 3
      },
      stock: [
        { id: '1', name: 'PVC Resin', unit: 'kg', reorderLevel: 500, availableKg: 1200 }
      ],
      efficiency: [
        { barrelNumber: 'B-01', producedKg: 900, hoursRun: 10, kgPerHour: 90, targetKgPerHour: 100 }
      ]
    };

    component.exportToExcel();

    expect(writeFileSpy).toHaveBeenCalled();
    const workbook = writeFileSpy.calls.most().args[0] as xlsx.WorkBook;
    expect(workbook.SheetNames).toEqual(['Summary', 'Raw Material Stock', 'Machine Efficiency']);
  });
});
