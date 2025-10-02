import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Navbar } from './navbar/navbar';
import { MainSidebar } from './main-sidebar/main-sidebar';
import { ControlSidebar } from './control-sidebar/control-sidebar';
import { Footer } from './footer/footer';

@Component({
  selector: 'app-layouts',
  imports: [
    RouterOutlet,
    Navbar,
    MainSidebar,
    ControlSidebar,
    Footer
  ],
  templateUrl: './layouts.html',
  styleUrl: './layouts.css'
})
export default class Layouts {

}
