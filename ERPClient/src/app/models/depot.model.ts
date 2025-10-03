export interface DepotModel {
    id: string,
    depotName: string,
    city: string,
    town: string,
    street: string
}

export const initialDepot: DepotModel = {
    id: "",
    depotName: "",
    city: "",
    town: "",
    street: ""
}