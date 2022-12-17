import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router }from '@angular/router';
import { Product } from '../Models/product.model';
import {ProductDataService} from '../Services/product-data.service';
import {Response} from '../Models/response.model';
//import { ProductItemComponent } from './product-item/product-item.component';
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})

export class ProductComponent implements OnInit {
  
  products:Product[];
  product:Product;
  public pId:string|null;
  constructor(public productoService:ProductDataService,public router:Router,public activatedRoute:ActivatedRoute ) { 
    if(this.router.url.includes('/Product') &&  this.activatedRoute.snapshot.paramMap.get('id')!=null)
    {
        this.pId = this.activatedRoute.snapshot.paramMap.get('id');
        
    }
  }

  ngOnInit(): void {
    this.pId = this.activatedRoute.snapshot.paramMap.get('id');
  }

}
