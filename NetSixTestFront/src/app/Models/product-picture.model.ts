import { Product } from "./product.model";

export class ProductPicture {
productPictureId:string;
fileName:string;
pictureData: Uint8Array = new Uint8Array(); 
hash:string | undefined;
productId:number;
product:Product|undefined;
}
