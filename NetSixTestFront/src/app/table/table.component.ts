import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { Product } from '../Models/product.model';
import { ProductDataService } from '../Services/product-data.service';
import { Response } from '../Models/response.model';
import { MatTableDataSource } from '@angular/material/table';
import {Sort, MatSortHeader,MatSort }from '@angular/material/sort';
import {ProductViewModel } from '../Models/product-view-model.model'
import{Router } from '@angular/router';
import { EntityEnum } from '../Models/entity-enum.model';
import { CategoryViewModel } from '../Models/category-view-model.model';
import { CategoryDataService } from '../Services/category-data.service';
import { Category } from '../Models/category.model';
@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent implements OnInit {

  
  
  dataSource :MatTableDataSource<ProductViewModel>=new MatTableDataSource<ProductViewModel>();
  dataSource1 :MatTableDataSource<CategoryViewModel>=new MatTableDataSource<CategoryViewModel>();
  displayColumns:string[]=[];
  displayColumns1:string[]=[];
  @Input()
  tableTitle:string;
  @Input()
  entity:string;
  @ViewChild(MatSort) sort: MatSort
  constructor(private router:Router, public productService:ProductDataService , public categoryDataService:CategoryDataService) {


   }


  ngAfterInit()
  {
    this.dataSource.sort = this.sort;
  }
  products:ProductViewModel[] =[];
  public categories:CategoryViewModel[]=[];
  ngOnInit(): void {

    this.loadData();
     


  }

 
  loadData(){
    this.displayColumns=
    [ 
      
      'id',
      'categoryName',
      'name',
      'quantity',
      'price',
      'enabled',
      'productIdUrl'
    ]
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

  sortData(sort:Sort){
    const data = this.products.slice();
      if(!sort.active || sort.direction===''){
          this.products = data;
          return;
      }

      const sortedData =data.sort((elA,elB)=>{
          const isAsc= sort.direction==='asc';
          switch(sort.active)
          {     
            case 'name':
             return this.compare(elA.name,elB.name,isAsc);
            case 'id':
             return this.compare(elA.id,elB.id,isAsc);
            case 'quantity':
              return this.compare(elA.quantity,elB.quantity,isAsc);
            case 'price':
              return  this.compare(elA.price,elB.price,isAsc);
            case 'categoryName':
              return  this.compare(elA.categoryName,elB.categoryName,isAsc);
            default:
               return 0;
          }
      });


  }
   compare(a: number | string, b: number | string, isAsc: boolean) {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
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
