import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Response } from '../Models/response.model';
import { Category } from '../Models/category.model';
@Injectable({
  providedIn: 'root'
})
export class CategoryDataService {

  categoryUrl:string='https://localhost:7170/Category';

  constructor(public httpClient:HttpClient) { 
      
  }
   getCategories = ()=>{
    return this.httpClient.get<Response>(this.categoryUrl);
   }
   createCategory = (category:Category) =>{
    return this.httpClient.post<Response>(this.categoryUrl,category);
   }
   updateCategory = (category:Category) =>{
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })};
      return this.httpClient.put<Response>(`${this.categoryUrl}/${category.id}`,category, httpOptions );
    }
   getCategory = (id:string|null)=>{
      return this.httpClient.get<Response>(`${this.categoryUrl}/${id}`);
   };
   deleteCategory(id:string){
    return this.httpClient.request<Response>('delete',`${this.categoryUrl}/${id}`);
   }
  }