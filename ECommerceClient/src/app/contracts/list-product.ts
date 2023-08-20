import { ListProductImage } from "./list-product-image";
import { Product } from "./product";

export class ListProduct extends Product {
    id: string;
    createdDate: Date;
    updatedDate: Date;
    productImageFiles?: ListProductImage[];
    imagePath: string;
}
