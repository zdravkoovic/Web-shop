import { Component, Inject, inject, OnDestroy, OnInit } from '@angular/core';
import { Product } from '../../interfaces/product';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { MatGridListModule } from '@angular/material/grid-list';
import { Observable} from 'rxjs';
import { ImageService } from '../../services/image.service';
import { SafeUrl } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { GalleryComponent } from '../gallery/gallery.component';
import { RatingComponent } from '../rating/rating.component';
import { NavProductDetailsComponent } from '../nav-product-details/nav-product-details.component';
import { CommentsService } from '../../services/comments.service';
import { Comments } from '../../interfaces/comments';
import { SignalRService } from '../../services/signal-r.service';

export interface Tile {
  url: SafeUrl;
  row: number;
  column: number;
}

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [ MatGridListModule, CommonModule, GalleryComponent, RatingComponent, NavProductDetailsComponent
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit, OnDestroy{
  product$?: Observable<Product | null>;

  comments: Comments[] | undefined = [];
  userComment: Comments | undefined;

  rating: number = 0;

  active = 'top';

  tiles: Tile[] = []

  private route = inject(ActivatedRoute);
  private router: Router = inject(Router);
  private productService: ProductService = inject(ProductService);
  private imgService: ImageService = inject(ImageService);
  private commentService: CommentsService = inject(CommentsService);

  constructor(
    private signalRService: SignalRService
  ){}

  ngOnInit(): void {
    console.log("Pozvan sam")
    let id : string | null = this.route.snapshot.paramMap.get("id");
    if(id == null) throw new Error("Nije nadjen odgovarajuci Id");
    this.product$ = this.productService.getProduct(Number(id));

    this.product$.subscribe(res => {

      if(res === null){
        this.router.navigate(['/404']);
        throw new Error("Product does not exist!");
      }

      let images = res?.images;
      
      if(images == undefined) {
        throw new Error("Slike nepostojece");
      }

      images!.forEach((el, index) => {
        this.imgService.download(el.path).subscribe(
          res => {
            if(index == 0){
              this.tiles.push({url: res, row: 4, column: 4})
              }else{
              this.tiles.push({url: res, row: 1, column: 1})
            }
          }
        )
      })

      this.signalRService.startConnection();

      this.commentService.getComments(res.id).subscribe(res => {
        
        if(res === null) return;

        this.comments = res.comments;
        this.comments.forEach(comment => comment.avatarUrl=this.createImageFromBase64(comment.avatarPicture))
        this.rating = res.rating;
        if(res.comment !== null){
          this.userComment = res.comment;
          this.userComment.avatarUrl = this.createImageFromBase64(this.userComment.avatarPicture);
        }
      });
    })
  }

  private createImageFromBase64(base64Image: string): string {
    return `data:image/jpeg;base64,${base64Image}`;
  }

  ngOnDestroy(): void {
    this.signalRService.endConnection();
    console.log('Connection is ended!')
  }
}

