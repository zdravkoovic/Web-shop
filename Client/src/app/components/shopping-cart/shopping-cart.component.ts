import { Component, NgIterable, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatMenu, MatMenuItem, MatMenuTrigger } from '@angular/material/menu';
import { Product } from '../../interfaces/product';
import { ProductService } from '../../services/product.service';
import { CommonModule } from '@angular/common';
import { map, Observable, of } from 'rxjs';
import { ShopingCartItemComponent } from "../shoping-cart-item/shoping-cart-item.component";
import { ShoppingCartItem } from '../../interfaces/shopping-cart-item';
import { MatBadgeModule } from '@angular/material/badge';

@Component({
  selector: 'app-shopping-cart',
  standalone: true,
  imports: [MatIconModule, MatButtonModule, MatMenu, MatMenuItem, MatMenuTrigger,
    CommonModule, ShopingCartItemComponent, MatIcon, MatBadgeModule],
  templateUrl: './shopping-cart.component.html',
  styleUrl: './shopping-cart.component.scss'
})
export class ShoppingCartComponent implements OnInit {
  shopping_products$?: Observable<ShoppingCartItem[]>;
  badgeNumber$?: Observable<number>;
  totalPrice$?: Observable<number>;
  displayBadge$: Observable<boolean> = of(true);

  constructor(private productService: ProductService){
  }
  ngOnInit(): void {
    this.shopping_products$ = this.productService.shopping_products$;

    this.badgeNumber$ = this.shopping_products$.pipe(
      map(items => items.length)
    );

    this.displayBadge$ = this.badgeNumber$.pipe(
      map(number => number == 0)
    );

    this.totalPrice$ = this.shopping_products$.pipe(
      map(products => products.reduce((sum, product) => sum + product.product.price*product.qunatity, 0))
    );  
  }
}
