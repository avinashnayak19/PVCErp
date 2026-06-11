export interface Dashboard {
  rawMaterialStockKg: number;
  productionApprovedKg: number;
  productionRejectedKg: number;
  scrapGeneratedKg: number;
  scrapReusedKg: number;
  pendingDispatchCount: number;
}

export interface RawMaterialStock {
  id: string;
  name: string;
  unit: string;
  reorderLevel: number;
  availableKg: number;
  location?: string;
}
export interface GrnStock {
  id: string;
  grnNumber: string;
  supplierId: string;
  supplierName: string;
  date: string;
  material: string;
  weight: string;
  qc: string;
}
export interface Supplier {
  id: string;
  name: string;
  country?: string;
  gstNumber?: string;
  phone?: string;
}

export interface Socketing {
  id: number;              // [Id] - primary key
  pipesReceived: number;   // [PipesReceived]
  pipesSocketed: number;   // [PipesSocketed]
  rejectedQty: number;     // [RejectedQty]
  shift: string;           // [Shift] - could be "Day", "Night", etc.
  scrapWeight: number;     // [ScrapWeight]
  entryDate: Date;         // [EntryDate]
  createdDate: Date;       // [CreatedDate]
  updatedDate?: Date;      // [UpdatedDate] - optional if nullable
  createdAtUtc: Date;      // [CreatedAtUtc]
  updatedAtUtc?: Date;     // [UpdatedAtUtc] - optional if nullable
}


export interface GrnItem {
  rawMaterialId: string;
  rawMaterialName?: string;
  batchNumber?: string;
  quantityKg?: number;
}

export interface Grn {
  id: string;
  grnNumber: string;
  supplierId: string;
  supplierName?: string;
  receiptDate: string;
  qualityStatus: number;
  items?: GrnItem[];
}

export interface BatchFormula {
  id: string;
  formulaCode: string;
  description: string;
  materialCount: number;
}

export interface Machine {
  id: string;
  machineNumber: string;
  description?: string;
}

export interface ScrewBarrel {
  id: string;
  barrelNumber: string;
  type?: string;
  targetKgPerHour: number;
}

export interface ProductionBatch {
  id: string;
  batchNumber: string;
  productionDate: string;
  shift: string;
  operatorName: string;
  approvedKg: number;
  rejectedKg: number;
  calciumKg?: number;
  resinKg?: number;
  pulverizerKg?: number;
  status?: string;
}

export type BatchHistory = ProductionBatch;

export interface Scrap {
  id: string;
  stage: number;
  scrapType: string;
  generatedKg: number;
  reusedKg: number;
  recoveryPercent: number;
}

export interface Efficiency {
  barrelNumber: string;
  producedKg: number;
  hoursRun: number;
  kgPerHour: number;
  targetKgPerHour: number;
}

export interface Customer {
  id: string;
  name: string;
  gstNumber?: string;
  phone?: string;
  address?: string;
}

export interface Dispatch {
  id: string;
  challanNumber: string;
  customerId: string;
  dispatchDate: string;
  status: number;
  vehicleNumber?: string;
  quantityKg: number;
}

export interface Invoice {
  id: string;
  invoiceNumber: string;
  dispatchChallanId: string;
  totalAmount: number;
  paymentStatus: number;
}
