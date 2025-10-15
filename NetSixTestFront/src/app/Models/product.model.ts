import { Category } from "./category.model";
import { ProductPicture } from "./product-picture.model";
export class Product {

    id:number=0;
    price:number=0.0;
    quantity:number=0;
    enabled:boolean=false;
    name:string='';
    categoryId:number=0;
    category:Category=new Category();
    pictures:ProductPicture[];
}
