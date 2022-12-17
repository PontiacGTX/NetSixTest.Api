import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { IndexComponent } from './index/index.component';
import { ProductComponent } from './product/product.component';
import { TableComponent } from './table/table.component';

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
  path:'Product/:id'
},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
