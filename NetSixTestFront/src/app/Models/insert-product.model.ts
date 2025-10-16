import { InsertProductPicture } from "./insert-product-picture.model";
import { ProductCategory } from "./product-category.model";

export class InsertProduct {
    name:string;
    price:number;
    quantity:number;
    enabled:boolean; 
    productPictures:InsertProductPicture[] = []
    productsCategories:ProductCategory[] = []
}
