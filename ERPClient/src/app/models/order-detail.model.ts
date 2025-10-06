import { initialProduct, ProductModel } from "./product.model"

export interface OrderDetailModel {

    id: string,
    orderId: string,
    productId: string,
    product: ProductModel,
    quantity: number
    price: number
   
}

export const initialOrderDetailModel:OrderDetailModel = {
    id: "",
    orderId: "",
    productId: "",
    product: { ...initialProduct },
    quantity: 0,
    price: 0
}