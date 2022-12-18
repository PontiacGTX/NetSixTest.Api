import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http'
import {Product }from '../Models/product.model'
import { Response } from '../Models/response.model';
import { HttpHeaders } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})

@Injectable()
export class ProductDataService {
 
  productsUrl:string='https://localhost:7170/Product';

  constructor(public httpClient:HttpClient) { 
      
  }
   getProducts = ()=>{
    return this.httpClient.get<Response>(this.productsUrl);
   }
   getProduct = (id:string|null)=>{
      return this.httpClient.get<Response>(`${this.productsUrl}/${id}`);
   };
   createProduct = (product:Product) =>{
    return this.httpClient.post<Response>(this.productsUrl,product);
   }
   updateProduct = (product:Product) =>{
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })};
    return this.httpClient.put<Response>(`${this.productsUrl}/${product.id}`,product, httpOptions );
   }
   deleteProduct(id:string){
    return this.httpClient.request<Response>('delete',`${this.productsUrl}/${id}`);
   }
}
