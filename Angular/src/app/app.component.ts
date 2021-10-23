import { IProduct } from 'src/app/shared/models/product';
import { IPagination } from './shared/models/pagination';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {
  title = 'TMall';

  product: IProduct;

  constructor(){}
  ngOnInit(): void {}
}
