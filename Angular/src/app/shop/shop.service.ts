import { IBrand, IProductType } from './../shared/models/Lookup';
import { IPagination } from './../shared/models/pagination';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators'; 

@Injectable({
  providedIn: 'root'
  // no need to add this to app module
})

export class ShopService {
 baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  getProducts(brandId?: number, typeId?: number) {
    // tslint:disable-next-line:prefer-const
    let params = new HttpParams();

    if (brandId){
      params.append('brandId', brandId.toString());
    }

    if (typeId){
      params.append('typeId', typeId.toString());
    }

    return this.http.get<IPagination>(this.baseUrl + 'products', {observe: 'response', params})
    .pipe(
      map(response => {
        return response.body;
      })
    );
    //    return this.http.get<IPagination>(this.baseUrl + 'products' )
    // this will gives us a response body and
    // the above will return http response => observe: 'response', instead of the body of the response
    // if so we neet to extract the body out of this by using .pipe()
    // inside  .pipe() we can use of RX-js methods ??
  }

  getBrands() {
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');
  }

  getProductTypes() {
    return this.http.get<IProductType[]>(this.baseUrl + 'products/types');
  }

}
