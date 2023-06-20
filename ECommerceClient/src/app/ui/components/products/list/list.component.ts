import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ListProduct } from 'src/app/contracts/list-product';
import { ProductService } from 'src/app/services/common/product.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

  constructor(private productService: ProductService, private activatedRoute: ActivatedRoute) { }

  isMainPage: boolean;
  currentPageNo: number;
  totalProductCount: number;
  totalPageCount: number;
  pageSize: number = 12;
  pageList: number[] = [];

  products: ListProduct[];

  ngOnInit() {
    this.activatedRoute.params.subscribe(async params => {

      if (params["page"] == null) {
        this.isMainPage = true;
        this.currentPageNo = 1;
      } else {
        this.currentPageNo = parseInt(params["page"]);
      }


      const data = await this.productService.getAll(this.currentPageNo - 1, this.pageSize, () => {

      }, errorMessage => {

      });

      this.products = data.products;

      this.products = this.products.map<ListProduct>(p => {
        const listProduct: ListProduct = {
          id: p.id,
          createdDate: p.createdDate,
          imagePath: p.productImageFiles.length ? p.productImageFiles.find(p => p.showcase).path : "",
          name: p.name,
          price: p.price,
          stock: p.stock,
          updatedDate: p.updatedDate,
          productImageFiles: p.productImageFiles
        };

        return listProduct;
      });

      this.totalProductCount = data.totalCount;
      this.totalPageCount = Math.ceil(this.totalProductCount / this.pageSize);
      this.pageList = [];

      if (this.currentPageNo - 3 <= 0)
        for (let i = 1; i <= this.totalPageCount; i++)
          this.pageList.push(i);

      else if (this.currentPageNo + 3 >= this.totalPageCount)
        for (let i = this.totalPageCount - 6; i <= this.totalPageCount; i++)
          this.pageList.push(i);
      else
        for (let i = this.currentPageNo - 3; i <= this.currentPageNo + 3; i++)
          this.pageList.push(i);
    });
  }


}
