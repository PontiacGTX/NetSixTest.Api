import { Component, OnInit } from '@angular/core';
import { Category } from 'src/app/Models/category.model';
import { Response } from 'src/app/Models/response.model';
import { CategoryViewModel } from 'src/app/Models/category-view-model.model';
import { MatTableDataSource } from '@angular/material/table';
import { CategoryDataService } from 'src/app/Services/category-data.service';
@Component({
  selector: 'app-category-table',
  templateUrl: './category-table.component.html',
  styleUrls: ['./category-table.component.css']
})
export class CategoryTableComponent implements OnInit {

  categoriesSrc: Category[]=[];
  data:CategoryViewModel[] =[];
  displayColumns1:string[] = [];
  dataSource1:CategoryViewModel[];
  constructor(public categoryDataService:CategoryDataService) {
  
   }

  ngOnInit(): void {
    this.loadData();
  }

  getCategoryKeys():string[]{
    return Object.getOwnPropertyNames(new CategoryViewModel());
  }
  public loadData =()=>{
    this.displayColumns1 =['id','enabled','name','categoryUrl'];
    this.categoryDataService.getCategories().subscribe((r:Response)=>{
     
      this.categoriesSrc  = r.data as Category[];
      
    });
    this.data = this.categoriesSrc.map((c:Category)=>
    ({
      'id':c.id,
      'name':c.name,
      'enabled':c.enabled
    } as CategoryViewModel)
);
    console.log(this.categoriesSrc);
    this.dataSource1 = (this.data);

    this.categoryDataService.getCategories().subscribe((d:Response )=>{
    
      this.data =   (d.data as Category[])
                        .map((c:Category)=>  ({
                        'id' : c.id,
                        'name':c.name,
                        'enabled':c.enabled,
                        'categoryUrl':`${c.id}`
                      } as CategoryViewModel) );
      this.dataSource1 = (this.data);
   });
  }
  onDeleteCategory(id:any){
    this.categoryDataService.deleteCategory(`${id}`)
    .subscribe((r:Response)=>{
      if(r.statusCode==200){
        this.ngOnInit();
        alert("Category deleted Successfully");
      }
    });
    this.loadData();
  }
}
