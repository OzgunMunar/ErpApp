import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { Auth } from './services/auth';

export const routes: Routes = [
    
    {
        path: "login",
        loadComponent: () => import("../app/components/login/login")
    },
    {
        path: "",
        loadComponent: () => import("../app/components/layouts/layouts"),
        canActivateChild: [() => inject(Auth).isAuthenticated()],
        children: 
        [
            {
                path: "",
                loadComponent: () => import("../app/components/home/home")
            },
            {
                path: "customers",
                loadComponent: () => import("../app/components/customers/customers")
            },
            {
                path: "depots",
                loadComponent: () => import("../app/components/depots/depots")
            },
            {
                path: "products",
                loadComponent: () => import("../app/components/products/products")
            },
            {
                path: "recipies",
                loadComponent: () => import("../app/components/recipies/recipies")
            },
            {
                path: "recipe-details/:id",
                loadComponent: () => import("../app/components/recipe-details/recipe-details")
            }
        ]
    }

];
