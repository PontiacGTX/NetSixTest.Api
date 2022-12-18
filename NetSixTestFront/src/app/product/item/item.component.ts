import { Component, OnInit,Input } from '@angular/core';
import { Product } from 'src/app/Models/product.model';
import { ProductDataService } from 'src/app/Services/product-data.service';
import { Response } from 'src/app/Models/response.model';


@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit {
  product:Product;
  constructor(public productoService:ProductDataService) {
  
   }
@Input() pId:string|null;
  ngOnInit(): void {
    console.log(this.pId);
    this.productoService.getProduct(this.pId)
    .subscribe((p:Response)=>{
      this.product = p.data as Product;
    });
  }

  
}
