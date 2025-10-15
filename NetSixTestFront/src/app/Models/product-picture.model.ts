import { Product } from "./product.model";

export class ProductPicture {
productPictureId:string;
fileName:string;
pictureData: number[] ; 
hash:string | undefined;
productId:number;
product:Product|undefined;
}
