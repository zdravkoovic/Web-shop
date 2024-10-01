import { Injectable, OnInit } from '@angular/core';
import { Product } from '../interfaces/product';
import { BehaviorSubject, concat, finalize, Observable, of, shareReplay, switchMap } from 'rxjs';
import { map } from 'rxjs'
import { HttpClient } from '@angular/common/http';
import { ShoppingCartItem } from '../interfaces/shopping-cart-item';
import { ImageService } from './image.service';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ProductService implements OnInit {

  private productMap: BehaviorSubject<Map<number, Product>> = new BehaviorSubject<Map<number, Product>>(new Map());
  products$ : Observable<Map<number, Product>> = this.productMap.asObservable();
  private productsCache?: Map<number, Product>; 

  private shoppingSubject: BehaviorSubject<ShoppingCartItem[]> = new BehaviorSubject<ShoppingCartItem[]>([]);
  shopping_products$: Observable<ShoppingCartItem[]> = this.shoppingSubject.asObservable();

  constructor(
    private http: HttpClient,  
    private imgService: ImageService  
  ) {}
  ngOnInit(): void {
    console.log('Product service instanciran iz OnInit');

  }
  
  getProducts() : Observable<Product[]>{
    return this.http.get<Product[]>("https://localhost:7211/products").pipe(
      map(products => 
        Array.from(products.values()))
    )
  }

  getProduct(id: number) : Observable<Product | null>{
    //mozda neka validacija

    return this.http.get<Product | null>("https://localhost:7211/product/"+id).pipe(
      map(res => {
        if(res === null) return null;
        res.images.forEach(el => {
          this.imgService.download(el.path).subscribe(url => el.safeUrl = url)
        })
        return res;
      }),
    )
  }

  shopOne(product: ShoppingCartItem): void{
    let list = this.shoppingSubject.getValue();
    let prod = list.find(prod => prod.product == product.product)
    
    if(prod) prod.qunatity+=1;
    else list.push(product)
    
    this.shoppingSubject.next(list);
  }

  deleteFromShopList(product: Product): void{
    let list = this.shoppingSubject.getValue();
    let prod = list.find(prod => prod.product == product);
    let index = list.indexOf(prod!);
    if(prod!.qunatity > 1) prod!.qunatity-=1
    else list.splice(index, 1)
    this.shoppingSubject.next(list);
  }

  GetByIdWithImages(id: string | null): Observable<Product | undefined>{
    if(id == null) throw "Id ne postoji";

    console.log(this.productMap.value);
    console.log(this.productsCache)
    return this.products$.pipe(
      map(data => {
        
        return data.get(Number(id))
      })
    )
  }

  getDescription(id: number) : Observable<string | null>{
    return this.http.get<string>(environment.baseUrl + "product-description/" + id, {responseType: 'text' as 'json'});
  }
}
