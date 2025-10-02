import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-blank',
  imports: [
    RouterLink
  ],
  templateUrl: './blank.html',
  styleUrl: './blank.css'
})
export class Blank {

  readonly routes = input<string[]>([]);
  readonly pageName = input<string>("");

}
