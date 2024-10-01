import { Component, Input, OnInit } from '@angular/core';
import { Product } from '../../interfaces/product';
import { CommonModule } from '@angular/common';
import { ImageService } from '../../services/image.service';
import { SafeUrl } from '@angular/platform-browser';
import { MatCard, MatCardActions, MatCardContent, MatCardImage, MatCardMdImage, MatCardSmImage, MatCardSubtitle, MatCardTitle } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { ProductService } from '../../services/product.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardContent, MatCardSmImage, MatCardTitle, MatCardSubtitle,
    MatCardActions, MatCard, MatCardImage, MatCardMdImage, MatCardContent, MatCardActions, MatButton, MatIcon
  ],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent implements OnInit{
  @Input() product!: Product
  @Input() path: string | undefined

  src: SafeUrl | undefined

  constructor(
    private image: ImageService,
    private productService: ProductService,
  ) {
  }

  ngOnInit(): void {
    if(this.path != undefined)
      {
        this.image.download(this.path).subscribe(
          r=>{
            this.src = r;
          }
        ); 
      }
  }

  isPathNull(path: string | undefined) : boolean { return path != undefined }

  addToCart(event: MouseEvent) : void{
    this.productService.shopOne({
      product: this.product,
      qunatity: 1 
    });
    event.stopPropagation();
  }
}
