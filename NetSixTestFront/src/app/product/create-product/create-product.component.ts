import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/Models/product.model';
import { CategoryDataService } from 'src/app/Services/category-data.service';
import { ProductDataService } from 'src/app/Services/product-data.service';
import { Response } from 'src/app/Models/response.model';
import { Category } from 'src/app/Models/category.model';
import { Router } from '@angular/router';
@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.css']
})
export class CreateProductComponent implements OnInit {

  product:Product= new Product();
  checkedEnabled:boolean=true;
  public categories:Category[];
  constructor(public categoryDataService:CategoryDataService, public productDataService:ProductDataService,public route:Router) {
    this.product.enabled=true;
    this.product.categoryId = 1;
    this.getCategories();
  }
  onOptionsSelected(value:any|null){

    this.product.categoryId = (value as number);
  }

  onSubmitProduct(prod:Product){
    console.log(prod);
    console.log(this.product);

    this.productDataService.createProduct(prod).subscribe((r:Response)=>{
      console.log(r);
      if(r.statusCode==200|| r.statusCode==201){
        alert("Sucessfully Create");
          this.route.navigateByUrl('/Product');
      }
    });
  }
  getCategories(){
    return  this.categoryDataService.getCategories().subscribe((r:Response)=>
    {
       
        this.categories = r.data as Category[];
    })
  }

  ngOnInit(): void {
    this.getCategories();
  }

}