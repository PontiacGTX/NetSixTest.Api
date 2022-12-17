import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Response } from '../Models/response.model';
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
   
   getCategory = (id:string|null)=>{
      return this.httpClient.get<Response>(`${this.categoryUrl}/${id}`);
   };
  }