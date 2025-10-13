import { CustomerModel, initialCustomer } from "./customer.model"
import { InvoiceDetailModel } from "./invoice-detail.model"
import { OrderDetailModel } from "./order-detail.model"

export interface InvoiceModel {

    id: string,
    invoiceNumber: string,
    orderedDate: string,
    deliveryDate: string,
    customerId: string,
    customer: CustomerModel,
    status: number,
    details: InvoiceDetailModel[]

}

export const initialInvoiceModel:InvoiceModel = {

    id: "",
    invoiceNumber: "",
    orderedDate: "",
    deliveryDate: "",
    customerId: "",
    customer: { ...initialCustomer },
    status: 0,
    details: []
}