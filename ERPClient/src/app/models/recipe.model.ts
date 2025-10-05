import { initialProduct, ProductModel } from "./product.model";
import { RecipeDetailModel } from "./recipe-detail.model";

export interface RecipeModel {

    id: string,
    productId: string,
    product: ProductModel,
    details: RecipeDetailModel[]

}

export const initialRecipe : RecipeModel = {

    id: "",
    productId: "",
    product: { ...initialProduct },
    details: []

}