export interface ProductModel {
    
    id: string,
    productName: string,
    productType: number,
    quantity: number

}

export const initialProduct: ProductModel = {
    id: "",
    productName: "",
    productType: 0,
    quantity: 0
}