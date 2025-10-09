import { ProductModel } from "./product.model"

export interface RequirementsPlanningModel {

    date: string,
    title: string,
    products: ProductModel[]

}

export const initialRequirementsPlanningModel: RequirementsPlanningModel = {
    date: "",
    title: "",
    products: []
}