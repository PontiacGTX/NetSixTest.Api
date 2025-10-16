import { Category } from "./category.model";
import { ProductPicture } from "./product-picture.model";
import { ProductsCategories } from "./products-categories.model";
export class Product {

    id:number=0;
    price:number=0.0;
    quantity:number=0;
    enabled:boolean=false;
    name:string=''; 
    pictures:ProductPicture[];
    productsCategories:ProductsCategories[];
}
