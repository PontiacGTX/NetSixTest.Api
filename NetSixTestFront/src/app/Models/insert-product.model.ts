import { InsertProductPicture } from "./insert-product-picture.model";

export class InsertProduct {
    name:string;
    price:number;
    quantity:number;
    enabled:boolean;
    categoryId:number;
    productPictures:InsertProductPicture[] = []
}
