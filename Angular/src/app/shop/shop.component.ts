import { IBrand, IProductType } from './../shared/models/Lookup';
import { ShopService } from './shop.service';
import { Component, OnInit } from '@angular/core';
import { IProduct } from '../shared/models/product';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styles: [
  ]
})
export class ShopComponent implements OnInit {

  products: IProduct[];
  brands: IBrand[];
  productTypes: IProductType[];
  brandIdSelected = 0;
  typeIdSelected = 0;
  sortSelected = 'name';
  sortOptions = [
    // tslint:disable-next-line:whitespace
    {name: 'Alphabetical', value:'name'},
    {name: 'price: low to high', value: 'priceAsc'},
    {name: 'price: high to low', value: 'priceDesc'}
  ];

  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  // tslint:disable-next-line:typedef
  getProducts() {
      this.shopService.getProducts(this.brandIdSelected, this.typeIdSelected, this.sortSelected)
      .subscribe(res => {
      this.products = res.data;
    }, error => {
      console.log(error);
    });
  }

  // tslint:disable-next-line:typedef
  getBrands() {
    this.shopService.getBrands().subscribe(res => {
      this.brands = [{id: 0, name: 'all'}, ...res];
      // ... spread operator than will allow adding element in the front of the array
    // this.brands = res;
  }, error => {
    console.log(error);
  });
}

  // tslint:disable-next-line:typedef
  getTypes() {
    this.shopService.getProductTypes()
    .subscribe(res => {
      this.productTypes = [{id: 0, name: 'all'}, ...res];
   // this.productTypes = res;
  }, error => {
    console.log(error);
  });

}

 onBrandSelected(brandId: number){

this.brandIdSelected = brandId ;
this.getProducts();
}

 onTypeSelected(typeId: number){
this.typeIdSelected = typeId ;
this.getProducts();
}

onSortSelected(sort: string){
this.sortSelected = sort ;
this.getProducts();
}


}
