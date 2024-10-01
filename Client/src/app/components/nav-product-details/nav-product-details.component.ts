import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgbNav, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { CommentsComponent } from '../comments/comments.component';
import { Comments } from '../../interfaces/comments';
import { RatingComponent } from "../rating/rating.component";
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { RateComponent } from "../rate/rate.component";
import { ProductService } from '../../services/product.service';
import { Product } from '../../interfaces/product';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-nav-product-details',
  standalone: true,
  imports: [NgbNavModule, CommentsComponent, RatingComponent, MatButtonModule, CommonModule, RateComponent],
  templateUrl: './nav-product-details.component.html',
  styleUrl: './nav-product-details.component.scss'
})
export class NavProductDetailsComponent implements OnInit, OnDestroy{
  @Input() comments: Comments[] | undefined;
  @Input() userComment: Comments | undefined;
  @ViewChild('nav') nav?: NgbNav;

  productId: number | null = null;

  description?: string | null;
  private descSubscription?: Subscription;
  
  active = 'description';


  constructor(
    private router: ActivatedRoute,
    private productService: ProductService
  ){}

  ngOnInit(): void {
    this.productId = Number(this.router.snapshot.paramMap.get("id"));

    if(this.productId != null)
    {
      this.descSubscription = this.productService.getDescription(this.productId).subscribe(res => this.description = res);
    }
  }

  ngOnDestroy(): void {
    this.descSubscription?.unsubscribe();
  }

  onNavigateToTab(tabId: string){
    this.nav!.select(tabId);
  }
}
