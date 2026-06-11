import { AsyncPipe, DecimalPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ApiService } from '../../core/api.service';

@Component({
  selector: 'app-production',
  standalone: true,
  imports: [AsyncPipe, DecimalPipe],
  templateUrl: './production.component.html',
  styleUrl: './production.component.css'
})
export class ProductionComponent {
  private readonly api = inject(ApiService);
  efficiency$ = this.api.getEfficiency();
}
