import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CRUDOpEnum } from '../Models/crudop-enum.model';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {

  public create:CRUDOpEnum = CRUDOpEnum.CREATE;
  public categoryId:string|null;

  public update:CRUDOpEnum = CRUDOpEnum.UPDATE;
  constructor(public router:Router,public activatedRoute:ActivatedRoute) { 
    this.categoryId =this.activatedRoute.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
  }

}
