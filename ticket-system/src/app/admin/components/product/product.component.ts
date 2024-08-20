import { APIResponse } from './../../../core/model/APIResponse';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule } from '@ngx-translate/core';
import { Product } from '../../../core/model/Product';
import { ProductService } from '../../../shared/services/product.service';
import { FormsModule } from '@angular/forms';
import { AlertboxService } from '../../../shared/services/alertbox.service';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule, TranslateModule, NgbPaginationModule, FormsModule],
  templateUrl: './product.component.html',
  styleUrl: './product.component.css',
})
export class ProductComponent implements OnInit {
  pageSize = 10;
  page = 1;
  products: Product[] = [];

  productName!: string;
  ngOnInit(): void {
    this.getProducts();
  }

  constructor(
    private pService: ProductService,
    private alert: AlertboxService
  ) {}
  addProduct() {
    const productExists = this.products.some(
      (product) => product.name === this.productName
    );

    if (productExists) {
      this.alert.error('Product name already exists!');
      return;
    }

    this.alert.ask('Are you sure want add the product?', () => {
      this.pService
        .addProduct(this.productName)
        .subscribe((res: APIResponse) => {
          this.alert.success('Successfull', 'Product Added Successfully');
          this.productName = '';
          this.getProducts();
        });
    });
  }
  getProducts() {
    this.pService.getPorudcts().subscribe((res: APIResponse) => {
      this.products = res.data;
    });
  }

  //start pagination
  onItemChange(event: any) {
    this.pageSize = event.target.value;
  }
}
