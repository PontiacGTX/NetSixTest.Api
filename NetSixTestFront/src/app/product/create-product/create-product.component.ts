import { Component, OnInit,Input } from '@angular/core';
import { Product } from 'src/app/Models/product.model';
import { CategoryDataService } from 'src/app/Services/category-data.service';
import { ProductDataService } from 'src/app/Services/product-data.service';
import { Response } from 'src/app/Models/response.model';
import { Category } from 'src/app/Models/category.model';
import { Router } from '@angular/router';
import { CRUDOpEnum } from 'src/app/Models/crudop-enum.model';
import { ActivatedRoute } from '@angular/router';
import { CdkRecycleRows } from '@angular/cdk/table';
@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.css']
})
export class CreateProductComponent implements OnInit {

  product:Product= new Product();
  checkedEnabled:boolean=true;
  @Input()
  public operation:CRUDOpEnum;
  public categories:Category[];
  constructor(public categoryDataService:CategoryDataService, public productDataService:ProductDataService,public route:Router,
    public activatedRoute:ActivatedRoute ) {
    this.product.enabled=true;
    this.product.categoryId = 1;
   
    this.operation =  this.route.url.includes('Create')  ?  CRUDOpEnum.CREATE:CRUDOpEnum.UPDATE;
    if(this.operation == CRUDOpEnum.UPDATE && this.activatedRoute.snapshot.paramMap.get('id')!=null)
    {
        productDataService.getProduct(this.activatedRoute.snapshot.paramMap.get('id'))
        .subscribe((r:Response)=>{
          this.product  = r.data as Product;
          this.getCategories();
        });
    }
    else
    {
      this.product.categoryId=0;
      this.getCategories();
      this.product.categoryId=this.categories!=null && this.categories.length>0 ? this.categories[0].id:0;
    }
  }
  onOptionsSelected(value:any|null){

    this.product.categoryId = (value as number);
  }
  
  canRedirect:Boolean=false;
  response:any;
  onSubmitProduct(prod:Product){
    console.log(prod);
    console.log(this.product);
   switch(this.operation)
   {
    case CRUDOpEnum.CREATE:
    this.productDataService.createProduct(prod).subscribe((r:Response)=>{
      console.log(r);
      if((r as Response).statusCode==200|| (r as Response).statusCode==201){
        this.canRedirect=true;
      }
      else
      {
        this.response = r;
      }
    });
    break;
    case CRUDOpEnum.UPDATE:
      this.productDataService.updateProduct(this.product).subscribe((r:any )=>{
        if((r as Response).statusCode==200)
        {
          this.canRedirect=true;
        }
        else
        this.response = r;
      });
      break;
      default:
        alert('operation not supported');
        this.route.navigateByUrl('/Product');
        break;
   }
    if(this.canRedirect || this.response==null){
      alert('success');
      this.route.navigateByUrl('/Product');
    }
    else
    {
        console.log(this.response);
        alert(this.response.validation.map((e:string)=>e).join(','));
    }
  }

  getCategories(){
    return  this.categoryDataService.getCategories().subscribe((r:Response)=>
    {
       
        this.categories = (r.data as Category[]).filter(c=>c.enabled || c.id == this.product.categoryId);
    })
  }

  ngOnInit(): void {
    this.getCategories();
  }

}