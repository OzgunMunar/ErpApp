import { initialProduct, ProductModel } from "./product.model";

export interface RecipeDetailModel {

    id: string,
    productId: string,
    product?: ProductModel,
    quantity: number

}

export const initialRecipeDetailModel : RecipeDetailModel = {

    id: "",
    productId: "",
    product: {...initialProduct},
    quantity: 1
    
}