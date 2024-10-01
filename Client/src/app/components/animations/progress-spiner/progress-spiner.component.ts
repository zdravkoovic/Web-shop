import { Component } from '@angular/core';
import { ProgressSpinnerService } from '../../../services/progress-spinner.service';
import { CommonModule } from '@angular/common';
import { MatProgressSpinner } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-progress-spiner',
  standalone: true,
  imports: [CommonModule, MatProgressSpinner],
  templateUrl: './progress-spiner.component.html',
  styleUrl: './progress-spiner.component.scss'
})
export class ProgressSpinerComponent {
  
  loading$ = this.service.loading$;

  constructor(private service: ProgressSpinnerService){}
}
