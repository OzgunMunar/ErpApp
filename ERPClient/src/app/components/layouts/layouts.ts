import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { Navbar } from './navbar/navbar';
import { MainSidebar } from './main-sidebar/main-sidebar';
import { ControlSidebar } from './control-sidebar/control-sidebar';
import { Footer } from './footer/footer';
import { FlexiToastService } from 'flexi-toast';

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
export default class Layouts{


}
