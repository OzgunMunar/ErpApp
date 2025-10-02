import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Blank } from '../components/blank/blank';
import { Section } from '../components/section/section';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
  ],
  imports: [

    CommonModule,
    Blank,
    Section,
    FormsModule

  ],
  exports:[

    CommonModule,
    Blank,
    Section,
    FormsModule

  ]
})

export class SharedModule { 
  
}
