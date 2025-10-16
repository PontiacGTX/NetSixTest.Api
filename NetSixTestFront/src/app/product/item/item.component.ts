import { Component, OnInit,Input } from '@angular/core';
import { Product } from 'src/app/Models/product.model';
import { ProductDataService } from 'src/app/Services/product-data.service';
import { Response } from 'src/app/Models/response.model';
import { ProductPicture } from 'src/app/Models/product-picture.model';


@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit {
  product:Product;
  constructor(public productoService:ProductDataService) {
  
   }


 public getCategoryNames(): string {
    if (!this.product || !this.product.productsCategories) {
      return '';
    }
    
    // Perform the logic here in TypeScript
    return this.product.productsCategories
      .map(pc => pc.category.name)
      .join(', ');
  }

  byteArrayToBase64(pic: ProductPicture): string {
    // 1. Get the MIME type based on the file extension
    const mimeType = this.getMimeTypeFromExtension(pic.fileName.split(".").pop() as string);
    
    // 2. Prepend the Data URL prefix to the raw Base64 data from the API
    console.log(pic.pictureData); 
    const final = `data:${mimeType};base64,${pic.pictureData}`;
    console.log(final)
    return final
  }

    

  getMimeTypeFromExtension(ext: string): string {
    const mimeTypes: { [key: string]: string } = {
      jpg: 'image/jpeg',
      jpeg: 'image/jpeg',
      png: 'image/png',
      gif: 'image/gif',
      webp: 'image/webp',
      svg: 'image/svg+xml',
    };

    const normalized = ext.trim().toLowerCase().replace('.', '');
    return mimeTypes[normalized] || 'image/jpeg'; // Default to image/jpeg for safety
  }
@Input() pId:string|null;
  ngOnInit(): void {
    console.log(this.pId);
    this.productoService.getProduct(this.pId)
    .subscribe((p:Response)=>{
      this.product = p.data as Product;
    });
  }

  
}
