import { Component, effect, inject, signal } from '@angular/core';
import { MenuModel, Menus } from '../../../menus';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../../services/auth';

@Component({
  selector: 'app-main-sidebar',
  imports: [
    RouterLink,
    RouterLinkActive,
    FormsModule
  ],
  templateUrl: './main-sidebar.html',
  styleUrl: './main-sidebar.css'
})

export class MainSidebar {

  readonly auth = inject(Auth)

  readonly menus = signal<MenuModel[]>(Menus)
  readonly search = signal<string>("")

}