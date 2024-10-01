import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { NgbRatingModule } from '@ng-bootstrap/ng-bootstrap';
import { RatingService } from '../../services/rating.service';

@Component({
  selector: 'app-rating',
  standalone: true,
  imports: [NgbRatingModule, CommonModule],
  templateUrl: './rating.component.html',
  styleUrl: './rating.component.scss'
})
export class RatingComponent {
  @Input() rating!: number
	@Input() readonly : boolean = true;
  selected = Math.random()*10;
	hovered = 0;

  constructor(
    private ratingService: RatingService
  ){}

  ariaValueText(current: number, max: number) {
		return `${current} out of ${max} hearts`;
	}

  onRateChange(newRate: number){
    this.ratingService.rate(newRate);
  }
}
