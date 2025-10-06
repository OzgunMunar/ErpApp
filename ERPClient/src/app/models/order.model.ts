import { CustomerModel, initialCustomer } from "./customer.model"
import { initialOrderDetailModel, OrderDetailModel } from "./order-detail.model"

export interface OrderModel {

    id: string,
    orderNumber: string,
    orderedDate: string,
    deliveryDate: string,
    customerId: string,
    customer: CustomerModel,
    details: OrderDetailModel[]
}

export const initialOrderModel:OrderModel = {
    id: "",
    orderNumber: "",
    orderedDate: "",
    deliveryDate: "",
    customerId: "",
    customer: { ...initialCustomer },
    details: [{...initialOrderDetailModel}]
}