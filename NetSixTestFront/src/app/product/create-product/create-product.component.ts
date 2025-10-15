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
import { ProductPicture } from 'src/app/Models/product-picture.model';
import { InsertProductPicture } from 'src/app/Models/insert-product-picture.model';
import { InsertProduct } from 'src/app/Models/insert-product.model';

export class FileItem {
  base64:string;
  name:string;
}
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
   
   
  }
  onOptionsSelected(value:any|null){

    this.product.categoryId = (value as number);
  }
  
  canRedirect:Boolean=false;
  response:any;
  public selectedImages: FileItem[] = [];

 onFilesSelected(event: Event): void {
     const input = event.target as HTMLInputElement; 
 
     if (input.files) {
       Array.from(input.files).forEach(file => {
         const reader = new FileReader();
         reader.onload = (e: any) => {
           this.selectedImages.push({
            name: file.name,
            base64: e.target.result,
          });  
         };
         reader.readAsDataURL(file);
       });
     }
   }
   
     base64ToByteArray(base64: string): Uint8Array {
  // Remove the data URL prefix if present
  const base64Data = base64.split(',')[1];
  const binaryString = window.atob(base64Data);
  const len = binaryString.length;
  const bytes = new Uint8Array(len);

  for (let i = 0; i < len; i++) {
    bytes[i] = binaryString.charCodeAt(i);
  }

  return bytes;
}
  onSubmitProduct(prod:Product){
    console.log(prod);
   if(this.product.categoryId==0)
   {
    alert('this products needs a valid category, please create one');
    return;
   }
   switch(this.operation)
   {
    case CRUDOpEnum.CREATE:
      let insertProduct :InsertProduct=new InsertProduct();
      insertProduct.productPictures =this.selectedImages.map((file, index) =>
                                                         {  
                                                              const base64 = file.base64;
                                                              console.log(base64)
                                                              //const byteArray = this.base64ToByteArray(base64); 
                                                              const pic: InsertProductPicture = {
                                                                fileName: file.name,
                                                                pictureData: base64.split(',')[1]
                                                              };
                                                                  return pic;
                                                        });
      insertProduct.categoryId = prod.categoryId;
      insertProduct.enabled = prod.enabled;
      insertProduct.name = prod.name;
      insertProduct.price =prod.price;    
      insertProduct.quantity = prod.quantity;                                             
      console.log(insertProduct);                                                   
      this.productDataService.createProduct(insertProduct).subscribe((r:Response)=>{
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
    this.operation =  this.route.url.includes('Create')  ?  CRUDOpEnum.CREATE:CRUDOpEnum.UPDATE;
    if(this.operation == CRUDOpEnum.UPDATE && this.activatedRoute.snapshot.paramMap.get('id')!=null)
    {
        this.productDataService.getProduct(this.activatedRoute.snapshot.paramMap.get('id'))
        .subscribe((r:Response)=>{
          this.product  = r.data as Product;
          console.log(this.product)
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

}