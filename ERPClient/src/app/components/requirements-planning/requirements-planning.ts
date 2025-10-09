import { Component, effect, signal } from '@angular/core';
import { initialRequirementsPlanningModel, RequirementsPlanningModel } from '../../models/requirements-planning.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-requirements-planning',
  imports: [
    CommonModule
  ],
  templateUrl: './requirements-planning.html',
  styleUrl: './requirements-planning.css'
})

export default class RequirementsPlanning {

  readonly data = signal<RequirementsPlanningModel>({ ...initialRequirementsPlanningModel })

  constructor() {

    effect(() => {

      console.log(this.data())
    })

  }


}
