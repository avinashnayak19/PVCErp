import { AsyncPipe, DecimalPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ApiService } from '../../core/api.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [AsyncPipe, DecimalPipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  private readonly api = inject(ApiService);
  dashboard$ = this.api.getDashboard();
}
