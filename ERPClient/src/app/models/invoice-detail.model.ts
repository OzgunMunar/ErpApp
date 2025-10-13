import { DepotModel, initialDepot } from "./depot.model"
import { initialProduct, ProductModel } from "./product.model"

export interface InvoiceDetailModel {

    id: string,
    invoiceId: string,
    productId: string,
    depotId: string,
    depot: DepotModel
    product: ProductModel,
    quantity: number
    price: number
   
}

export const initialOrderDetailModel:InvoiceDetailModel = {
    id: "",
    invoiceId: "",
    productId: "",
    depotId: "",
    depot: { ...initialDepot },
    product: { ...initialProduct },
    quantity: 0,
    price: 0
}