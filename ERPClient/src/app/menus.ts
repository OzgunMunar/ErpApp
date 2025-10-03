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
    }

]