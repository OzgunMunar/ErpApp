export class MenuModel {
    name: string = "";
    icon: string = "";
    url: string = "";
    isTitle: boolean = false
}

export const Menus: MenuModel[] = [

    {
        name: "Ana Sayfa",
        icon: "fas fa-solid fa-home",
        url: "/",
        isTitle: false
    },   
    {
        name: "Customers",
        icon: "far fa fa-users",
        url: "/customers",
        isTitle: false
    },   
    {
        name: "Depots",
        icon: "far fa fa-warehouse",
        url: "/depots",
        isTitle: false
    },   
    {
        name: "Products",
        icon: "far fa fa-boxes",
        url: "/products",
        isTitle: false
    },   
    {
        name: "Recipies",
        icon: "far fa fa-scroll",
        url: "/recipies",
        isTitle: false
    }

]