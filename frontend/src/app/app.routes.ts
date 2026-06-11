import { Routes } from '@angular/router';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { InventoryComponent } from './features/inventory/inventory.component';
import { ProductionComponent } from './features/production/production.component';
import { DispatchComponent } from './features/dispatch/dispatch.component';
import { WorkflowComponent } from './features/workflow/workflow.component';
import { BatchManagementComponent } from './features/batch-management/batch-management.component';
import { SocketingComponent } from './features/socketing/socketing.component';
import { ScrapManagementComponent } from './features/scrap-management/scrap-management.component';
import { InvoiceComponent } from './features/invoice/invoice.component';
import { MachineEfficiencyComponent } from './features/machine-efficiency/machine-efficiency.component';
import { ReportsComponent } from './features/reports/reports.component';

export const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'workflow', component: WorkflowComponent },
  { path: 'inventory', component: InventoryComponent },
  { path: 'batch-management', component: BatchManagementComponent },
  { path: 'production', component: ProductionComponent },
  { path: 'socketing', component: SocketingComponent },
  { path: 'scrap-management', component: ScrapManagementComponent },
  { path: 'dispatch', component: DispatchComponent },
  { path: 'invoice', component: InvoiceComponent },
  { path: 'machine-efficiency', component: MachineEfficiencyComponent },
  { path: 'reports', component: ReportsComponent },
  { path: '**', redirectTo: '' }
];
