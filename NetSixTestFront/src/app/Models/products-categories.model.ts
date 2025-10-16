import { Category } from "./category.model";
import { Product } from "./product.model";

export class ProductsCategories {
    id:string;
    productId:number;
    product:Product;
    categoryId:number;
    category:Category; 
}
