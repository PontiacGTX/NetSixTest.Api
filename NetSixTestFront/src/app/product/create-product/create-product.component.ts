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
import { ProductCategory } from 'src/app/Models/product-category.model';
import { ProductsCategories } from 'src/app/Models/products-categories.model';
import { map, Observable } from 'rxjs';

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
   
   
  }
 
  canRedirect:Boolean=false;
  response:any;
  public selectedImages: FileItem[] = [];

   categoriesOptions :Category[] ;
   selectedCategories :Category[]=[];
 
  onOptionToggle(option: Category, el: any): void {
  let element = el as HTMLInputElement;
  if (element.checked) {
    if (!this.selectedCategories.includes(option))
      this.selectedCategories.push(option);
  } else {
    this.selectedCategories = this.selectedCategories.filter(o => o !== option);
  }
}
  onFilesSelected(event: Event): void {
    const input = event.target as HTMLInputElement; 
    const files = input.files;

    if (files) {
      Array.from(files).forEach(file => {
        const reader = new FileReader();

        
        reader.onload = (e: any) => {
           
          this.selectedImages.push({
            name: file.name,
            base64: e.target.result as string,  
          });
        };

         
        reader.readAsDataURL(file);
      });
    }
     
    input.value = '';
  }

    base64ToByteArray(base64String: string): Uint8Array {
  // Remove the data URL prefix if present (e.g. data:image/jpeg;base64,)
  const base64Data = base64String.split(',')[1] || base64String;

  // Decode base64 string to a binary string
  const binaryString = window.atob(base64Data);

  // Create Uint8Array with the size of the binary string length
  const len = binaryString.length;
  const bytes = new Uint8Array(len);

  // Convert each char into a byte
  for (let i = 0; i < len; i++) {
    bytes[i] = binaryString.charCodeAt(i);
  }

  return bytes;
}

  onSubmitProduct(prod:Product){
    console.log(prod);
   if(this.selectedCategories.length==0)
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
                                                              const pic: InsertProductPicture = {
                                                                fileName: file.name,
                                                                pictureData: base64.split(',')[1]
                                                              };
                                                                  return pic;
                                                        });
      insertProduct.productsCategories = this.selectedCategories.map(pc=> {
                                                                        return {
                                                                          productId:0,
                                                                          categoryId:pc.id
                                                                        }
                                                                    })    
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
         this.product.productsCategories = this.selectedCategories.map(pc=> {
                                                                        let p:ProductsCategories= new ProductsCategories();
                                                                        p.categoryId = pc.id;
                                                                        p.productId =this.product.id 
                                                                        return p;
                                                                    })    
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
        console.log("res" +this.response);
        alert(this.response.validation.map((e:string)=>e).join(','));
    }
  }

  getCategories():Observable<Category[]>{
    return   this.categoryDataService.getCategories().pipe(
                                                            map((r: Response) => {
                                                                    this.categories =r.data as Category[]
                                                                this.categoriesOptions =this.categories;
                                                                    return this.categories;
                                                                })
                                                          );
  }

  ngOnInit(): void {
    this.operation =  this.route.url.includes('Create')  ?  CRUDOpEnum.CREATE:CRUDOpEnum.UPDATE;
    if(this.operation == CRUDOpEnum.UPDATE && this.activatedRoute.snapshot.paramMap.get('id')!=null)
    {
        this.productDataService.getProduct(this.activatedRoute.snapshot.paramMap.get('id'))
        .subscribe((r:Response)=>{
          this.product  = r.data as Product; 
          this.getCategories().subscribe(cats=>{
               if(this.product.productsCategories!=null)
                  this.selectedCategories = this.categories.filter(cat=> this.product.productsCategories.some(pc => pc.categoryId === cat.id))
                  console.log( this.selectedCategories)
               });
          })
         
    }
    else
    { 
      this.getCategories().subscribe(cats=>{
            this.categories=cats;
      }); 
     
    }
  }

}