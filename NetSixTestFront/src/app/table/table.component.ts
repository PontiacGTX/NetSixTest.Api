import { Component, OnInit, ViewChild } from '@angular/core';
import { Product } from '../Models/product.model';
import { ProductDataService } from '../Services/product-data.service';
import { Response } from '../Models/response.model';
import { MatTableDataSource } from '@angular/material/table';
import {Sort, MatSortHeader,MatSort }from '@angular/material/sort';
import {ProductViewModel } from '../Models/product-view-model.model'
import{Router } from '@angular/router';
@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent implements OnInit {

  
  
  dataSource :MatTableDataSource<ProductViewModel>=new MatTableDataSource<ProductViewModel>();
  displayColumns:string[]=[];
  
  @ViewChild(MatSort) sort: MatSort
  constructor(private router:Router, public productService:ProductDataService ) {


   }


  ngAfterInit()
  {
    this.dataSource.sort = this.sort;
  }
  products:ProductViewModel[] =[];
  ngOnInit(): void {
    this.displayColumns=
    [ 
      'categoryName',
      'enabled',
      'id',
      'name',
      'price',
      'quantity',
      'productIdUrl'
    ]//Object.getOwnPropertyNames(new ProductViewModel())
    this.loadData();
    
   

  }

  loadData(){
    this.productService.getProducts().subscribe((d:Response )=>{
    
      this.products =   (d.data as Product[])
                        .map((p:Product)=>  ({
                        'id' : p.id,
                        'categoryName':p.category.name,
                        'enabled':p.enabled,
                        'price':p.price,
                        'quantity':p.quantity,
                        'name':p.name,
                        'productIdUrl':`${p.id}`
                      } as ProductViewModel) );
      this.dataSource = new MatTableDataSource<ProductViewModel>(this.products);
  
     })
  }

  onDeleteProduct(id:any){

    this.productService.deleteProduct(`${id}`)
    .subscribe((r:Response)=>{
      if(r.statusCode==200){
        this.ngOnInit();
        alert("Product deleted Successfully");
      }
    });
    this.loadData();
  //   this.router.navigateByUrl('/RefreshComponent', { skipLocationChange: true }).then(() => {
  //     this.router.navigate(['Your actualComponent']);
  // }); 
  }

}
