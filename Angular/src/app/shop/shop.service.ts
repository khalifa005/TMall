import { IPagination } from './../shared/models/pagination';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
  // no need to add this to app module
})

export class ShopService {
 baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  // tslint:disable-next-line:typedef
  getProducts() {
    return this.http.get<IPagination>(this.baseUrl + 'products? pageSize=50');
  }

}
