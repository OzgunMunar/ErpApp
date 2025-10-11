import { Component, effect, inject, signal } from '@angular/core';
import { initialRequirementsPlanningModel, RequirementsPlanningModel } from '../../models/requirements-planning.model';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Http } from '../../services/http';
import { ResultModel } from '../../models/result.model';
import { FlexiToastService } from 'flexi-toast';

@Component({
  selector: 'app-requirements-planning',
  imports: [
    CommonModule
  ],
  templateUrl: './requirements-planning.html',
  styleUrl: './requirements-planning.css'
})

export default class RequirementsPlanning {

  constructor(
    private activated: ActivatedRoute
  ) {

    this.activated.params.subscribe(res => {

      this.orderId.set(res["orderId"])
      this.get()

    })

  }

  readonly data = signal<RequirementsPlanningModel>({ ...initialRequirementsPlanningModel })
  readonly orderId = signal<string>("")

  readonly #http = inject(Http)
  readonly #toast = inject(FlexiToastService)

  get() {

    this.#http.post<RequirementsPlanningModel>("order/requirements-planning", { OrderId: this.orderId() }, 
      (res) => {

        if(res.data) {

          this.data.set(res.data)
          console.log(this.data())

        }

    })

  }

}
