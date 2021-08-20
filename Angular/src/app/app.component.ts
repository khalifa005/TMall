import { IPagination } from './shared/models/pagination';
import { IProduct } from './shared/models/product';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {
  title = 'TMall';

  // products: any[];
  products: IProduct[];

  constructor(private http: HttpClient){}
  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/products')
    .subscribe((response: IPagination) => {
      this.products = response.data;
      console.log(response);
    },
     error => {
      console.log(error);
    });
  }
}
