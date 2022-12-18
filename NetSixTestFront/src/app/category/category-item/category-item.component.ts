import { Component, OnInit,Input } from '@angular/core';
import { Category } from 'src/app/Models/category.model';
import { CategoryDataService } from 'src/app/Services/category-data.service';
import { Response } from 'src/app/Models/response.model';
@Component({
  selector: 'app-category-item',
  templateUrl: './category-item.component.html',
  styleUrls: ['./category-item.component.css']
})
export class CategoryItemComponent implements OnInit {

  constructor(public categoryDataService:CategoryDataService) {
      
  
   }

   loadData(){
      this.categoryDataService.getCategory(this.categoryId).subscribe((r:Response)=>{
       this.category =  r.data as Category;
      });
   }
  category:Category;
  @Input()
  public categoryId:string|null;
  ngOnInit(): void {
    this.loadData(); 
  }

}
