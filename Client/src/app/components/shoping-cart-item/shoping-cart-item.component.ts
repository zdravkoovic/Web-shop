import { Component, Input, OnInit } from '@angular/core';
import { MatIconButton } from '@angular/material/button';
import { Product } from '../../interfaces/product';
import { SafeUrl } from '@angular/platform-browser';
import { ImageService } from '../../services/image.service';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product.service';
import { ShoppingCartItem } from '../../interfaces/shopping-cart-item';

@Component({
  selector: 'app-shoping-cart-item',
  standalone: true,
  imports: [MatIconButton, MatIconModule, CommonModule],
  templateUrl: './shoping-cart-item.component.html',
  styleUrl: './shoping-cart-item.component.scss'
})
export class ShopingCartItemComponent implements OnInit{
  @Input() product!: ShoppingCartItem;
  path?: SafeUrl

  constructor(
    private imgService: ImageService,
    private productService: ProductService){
  }
  ngOnInit(): void {
    
    this.imgService.download(this.product.product.images.at(0)!.path).subscribe(res=> this.path = res)
  }

  onMenuItemClick(event: MouseEvent): void{
    event.stopPropagation();
    this.deleteProductFromShopList();
  }

  deleteProductFromShopList(): void{
    this.productService.deleteFromShopList(this.product.product);
  }
}
