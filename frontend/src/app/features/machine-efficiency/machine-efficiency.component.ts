import { AsyncPipe, DecimalPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ApiService } from '../../core/api.service';

@Component({
  selector: 'app-machine-efficiency',
  standalone: true,
  imports: [AsyncPipe, DecimalPipe],
  templateUrl: './machine-efficiency.component.html',
  styleUrl: './machine-efficiency.component.css'
})
export class MachineEfficiencyComponent {
  private readonly api = inject(ApiService);
  efficiency$ = this.api.getEfficiency();
}
