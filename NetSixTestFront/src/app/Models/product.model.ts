import { Category } from "./category.model";
export class Product {

    id:Number=0;
    price:Number=0.0;
    quantity:Number=0;
    enabled:Boolean=false;
    name:String='';
    categoryId:Number=0;
    category:Category=new Category();
}
