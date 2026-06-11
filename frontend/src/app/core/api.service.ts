import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import {
  BatchFormula,
  BatchHistory,
  Customer,
  Dashboard,
  Dispatch,
  Efficiency,
  Grn,
  GrnStock,
  Invoice,
  Machine,
  ProductionBatch,
  RawMaterialStock,
  Scrap,
  ScrewBarrel,
  Supplier,
  Socketing
} from './api.models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly baseUrl = 'https://localhost:7180/api';

  constructor(private readonly http: HttpClient) {}

  getDashboard(): Observable<Dashboard> {
    return this.http.get<Dashboard>(`${this.baseUrl}/reports/dashboard`);
  }

  getStock(): Observable<RawMaterialStock[]> {
    return this.http.get<RawMaterialStock[]>(`${this.baseUrl}/inventory/stock`);
  }

  getGrnstock(): Observable<GrnStock[]> {
    return this.http.get<GrnStock[]>(`${this.baseUrl}/inventory/GetGrn`);
  }

  getGrns(): Observable<Grn[]> {
    return this.http.get<Grn[]>(`${this.baseUrl}/inventory/grns`);
  }
  getRawmaterials(): Observable<RawMaterialStock[]> {
    // backend exposes materials at /inventory/get-materials
    return this.http.get<RawMaterialStock[]>(`${this.baseUrl}/inventory/get-materials`);
  }

  createRawMaterial(payload: { name: string; unit: string; reorderLevel: number; location?: string }): Observable<RawMaterialStock> {
    return this.http.post<RawMaterialStock>(`${this.baseUrl}/inventory/raw-materials`, payload);
  }

  createSupplier(payload: { name: string; country?: string; gstNumber?: string; phone?: string }): Observable<Supplier> {
    return this.http.post<Supplier>(`${this.baseUrl}/inventory/suppliers`, payload);
  }
  createSocketing(payload: {
    pipesReceived: number;
    pipesSocketed: number;
    rejectedQty: number;
    scrapWeight: number;
    shift: string;
    entryDate: string;
    createdAtUtc: string;
  }): Observable<Socketing> {
    return this.http.post<Socketing>(`${this.baseUrl}/Socketing/insertsocket`, payload);
  }
getSocketing(): Observable<Socketing[]> {
    return this.http.get<Socketing[]>(`${this.baseUrl}/Socketing/GetSocketing`);
  }
  createGrn(payload: {
    supplierId: string;
    receiptDate: string;
    qualityStatus: number;
    items: Array<{ rawMaterialId: string; batchNumber: string; quantityKg: number }>;
  }): Observable<Grn> {
    return this.http.post<Grn>(`${this.baseUrl}/inventory/grns`, payload);
  }

  createFormula(payload: {
    formulaCode: string;
    description: string;
    items: Array<{ rawMaterialId: string; standardQuantityKg: number }>;
  }): Observable<BatchFormula> {
    return this.http.post<BatchFormula>(`${this.baseUrl}/production/formulas`, payload);
  }

  createMachine(payload: { machineNumber: string; description?: string }): Observable<Machine> {
    return this.http.post<Machine>(`${this.baseUrl}/production/machines`, payload);
  }

  createScrewBarrel(payload: { barrelNumber: string; type?: string; targetKgPerHour: number }): Observable<ScrewBarrel> {
    return this.http.post<ScrewBarrel>(`${this.baseUrl}/production/screw-barrels`, payload);
  }

  getBatches(): Observable<ProductionBatch[]> {
    return this.http.get<ProductionBatch[]>(`${this.baseUrl}/production/batches`);
  }

  getBatchHistory(): Observable<BatchHistory[]> {
    return this.http.get<BatchHistory[]>(`${this.baseUrl}/batch/history`).pipe(
      map(data => data ?? [])
    );
  }

  createProductionBatch(payload: {
    batchFormulaId: string;
    productionDate: string;
    shift: string;
    operatorName: string;
    supervisorName?: string;
    consumptions: Array<{ rawMaterialId: string; standardQuantityKg: number; actualQuantityKg: number }>;
    outputs: Array<{
      machineId: string;
      screwBarrelId: string;
      stage: number;
      pipeDimension: string;
      approvedQuantityKg: number;
      rejectedQuantityKg: number;
      hoursRun: number;
    }>;
  }): Observable<ProductionBatch> {
    return this.http.post<ProductionBatch>(`${this.baseUrl}/production/batches`, payload);
  }

  recordScrap(payload: {
    stage: number;
    scrapType: string;
    generatedKg: number;
    reusedKg: number;
    sourceReference?: string;
    recordDate: string;
  }): Observable<Scrap> {
    return this.http.post<Scrap>(`${this.baseUrl}/production/scrap`, payload);
  }

  getEfficiency(): Observable<Efficiency[]> {
    return this.http.get<Efficiency[]>(`${this.baseUrl}/production/screw-barrel-efficiency`);
  }

  createCustomer(payload: { name: string; gstNumber?: string; phone?: string; address?: string }): Observable<Customer> {
    return this.http.post<Customer>(`${this.baseUrl}/dispatch/customers`, payload);
  }

  createDispatch(payload: {
    customerId: string;
    dispatchDate: string;
    vehicleNumber?: string;
    items: Array<{ pipeDimension: string; quantityKg: number; quantityPieces: number }>;
  }): Observable<Dispatch> {
    return this.http.post<Dispatch>(`${this.baseUrl}/dispatch/challans`, payload);
  }

  createInvoice(payload: { dispatchChallanId: string; invoiceDate: string; taxableAmount: number; gstAmount: number }): Observable<Invoice> {
    return this.http.post<Invoice>(`${this.baseUrl}/dispatch/invoices`, payload);
  }
}
