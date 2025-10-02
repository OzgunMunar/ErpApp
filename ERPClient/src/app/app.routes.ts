import { Routes } from '@angular/router';

export const routes: Routes = [
    
    {
        path: "login",
        loadComponent: () => import("../app/components/login/login")
    },
    {
        path: "",
        loadComponent: () => import("../app/components/layouts/layouts"),
        children: 
        [
            {
                path: "",
                loadComponent: () => import("../app/components/home/home")
            }
        ]
    }

];
