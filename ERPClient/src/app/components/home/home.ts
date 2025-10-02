import { Component } from '@angular/core';
import { Blank } from '../blank/blank';
import { Section } from '../section/section';
import { SharedModule } from '../../modules/shared-module';

@Component({
  selector: 'app-home',
  imports: [
    SharedModule
  ],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export default class Home {

}
