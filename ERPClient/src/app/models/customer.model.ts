export interface CustomerModel {
    id: string,
    fullName: string,
    taxDepartment: string,
    taxNumber: string,
    city: string,
    town: string,
    street: string
}

export const initialCustomer: CustomerModel = {
    id: "",
    fullName: "",
    taxDepartment: "",
    taxNumber: "",
    city: "",
    town: "",
    street: ""
}