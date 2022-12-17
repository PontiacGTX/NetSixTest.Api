import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HeaderComponent } from './header/header.component';
// import { TableComponent } from './table/table.component';
//import { MatTableModule } from '@angular/material/table' 
import { MatTableModule } from '@angular/material/table'  
import { TableComponent } from './table/table.component';
import { ProductComponent } from './product/product.component';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { IndexComponent } from './index/index.component';
import { FormsModule } from '@angular/forms';
import { ItemComponent } from './product/item/item.component';
import { CreateProductComponent } from './Product/create-product/create-product.component';
import { MatSelectModule } from '@angular/material/select';
import { ReactiveFormsModule } from '@angular/forms';
// import { MatSortModule } from '@angular/material/sort';
@NgModule({
  imports: [
    MatTableModule,
    BrowserModule,
    BrowserAnimationsModule,//needed for select animation
    AppRoutingModule,
    NgbModule,
    NgbDropdownModule,
    FormsModule,
    HttpClientModule,
    MatSelectModule,
    ReactiveFormsModule
  ],
  declarations: [
    AppComponent,
    HeaderComponent,
    TableComponent,
    ProductComponent,
    IndexComponent,
    ItemComponent,
    CreateProductComponent
    
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule { }
