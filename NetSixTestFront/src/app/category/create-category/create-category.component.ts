import { Component, OnInit,Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Category } from 'src/app/Models/category.model';
import { CRUDOpEnum } from 'src/app/Models/crudop-enum.model';
import { Response } from 'src/app/Models/response.model';
import { CategoryDataService } from 'src/app/Services/category-data.service';

@Component({
  selector: 'app-create-category',
  templateUrl: './create-category.component.html',
  styleUrls: ['./create-category.component.css']
})
export class CreateCategoryComponent implements OnInit {

  public category:Category = new Category();
  @Input()
  public operation:CRUDOpEnum;
  constructor(public categoryService:CategoryDataService,public route:Router,public activatedRoute:ActivatedRoute) { }

  ngOnInit(): void {
    if(this.activatedRoute.snapshot.paramMap.get('id')!=null){

      this.categoryService.getCategory(this.activatedRoute.snapshot.paramMap.get('id')).subscribe((r:Response)=>{
        this.category = r.data as Category;
      })
    }
  }
  response:any;
  canRedirect:boolean=false;
  onSubmitCategory(cat:Category){
   switch(this.operation)
   {
    case CRUDOpEnum.CREATE:
    this.categoryService.createCategory(cat).subscribe((r:Response)=>{
      console.log(r);
      if((r as Response).statusCode==200|| (r as Response).statusCode==201){
        this.canRedirect=true;
      }
      else
      {
        this.response = r;
      }
    });
    break;
    case CRUDOpEnum.UPDATE:
      this.categoryService.updateCategory(this.category).subscribe((r:any )=>{
        if((r as Response).statusCode==200)
        {
          this.canRedirect=true;
        }
        else
        this.response = r;
      });
      break;
      default:
        alert('operation not supported');
        this.route.navigateByUrl('/Category');
        break;
   }
    if(this.canRedirect || this.response==null){
      alert('success');
      this.route.navigateByUrl('/Category');
    }
    else
    {
        console.log(this.response);
        alert(this.response.validation.map((e:string)=>e).join(','));
    }
  }
}
