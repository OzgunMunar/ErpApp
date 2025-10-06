import { CustomerModel, initialCustomer } from "./customer.model"
import { OrderDetailModel } from "./order-detail.model"

export interface OrderModel {

    id: string,
    orderNumber: string,
    orderedDate: string,
    deliveryDate: string,
    customerId: string,
    customer: CustomerModel,
    status: number,
    details: OrderDetailModel[]

}

export const initialOrderModel:OrderModel = {

    id: "",
    orderNumber: "",
    orderedDate: "",
    deliveryDate: "",
    customerId: "",
    customer: { ...initialCustomer },
    details: [],
    status: 0

}