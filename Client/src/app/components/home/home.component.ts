import { Component, NgIterable, OnInit } from '@angular/core';
import { ImageService } from '../../services/image.service';
import { SafeUrl } from '@angular/platform-browser';
import { ProductsComponent } from '../products/products.component';
import { ProductService } from '../../services/product.service';
import { Product } from '../../interfaces/product';
import { debounceTime, distinctUntilChanged, finalize, map, Observable, OperatorFunction, tap } from 'rxjs';
import { AvatarImageComponent } from '../avatar-image/avatar-image.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProgressSpinerComponent } from '../animations/progress-spiner/progress-spiner.component';
import { ProgressSpinnerService } from '../../services/progress-spinner.service';
import { ProductDetailsComponent } from '../product-details/product-details.component';
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';



@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule,
    AvatarImageComponent, ProductsComponent, ProductDetailsComponent ,ProgressSpinerComponent,
    NgbTypeaheadModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  path: SafeUrl | undefined
  products$?: Observable<NgIterable<Product>>;
  proizvod: Product | any;
  model: any;

  
  constructor( 
    private productService: ProductService,
    private spinner: ProgressSpinnerService
  ){}

  ngOnInit(): void {
    this.fetchProducts();
    this.spinner.show();
  }

  fetchProducts() : void {
      
    this.products$ = this.productService.getProducts().pipe(
      finalize(() => {
          
        this.spinner.hide();

      })
    );
  }
}
