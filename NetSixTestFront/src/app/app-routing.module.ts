import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { CategoryComponent } from './category/category.component';
import { ProductComponent } from './product/product.component';

const routes: Routes = [{
  component:ProductComponent,
  path:'Product'
},
{
  component:ProductComponent,
  path:'Product/Create'
},
{
  component:ProductComponent,
  path:'Product/Edit/:id'
},
{
  component:ProductComponent,
  path:'Product/:id'
},
{
  component:CategoryComponent,
  path:'Category'
},
{
  component:CategoryComponent,
  path:'Category/Create'
},
{
  component:CategoryComponent,
  path:'Category/Edit/:id'
},
{
  component:CategoryComponent,
  path:'Category/:id'
},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
