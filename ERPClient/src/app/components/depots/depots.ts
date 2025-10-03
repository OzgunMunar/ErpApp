import { Component, computed, effect, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { Section } from '../section/section';
import { Blank } from '../blank/blank';
import { DepotModel, initialDepot } from '../../models/depot.model';
import { httpResource } from '@angular/common/http';
import { FlexiGridModule } from 'flexi-grid';
import { FormsModule, NgForm } from '@angular/forms';
import { FlexiToastService } from 'flexi-toast';
import { Http } from '../../services/http';
import * as bootstrap from 'bootstrap';
import { ResultModel } from '../../models/result.model';

@Component({
  selector: 'app-depots',
  imports: [
    Section,
    Blank,
    FlexiGridModule,
    FormsModule
  ],
  templateUrl: './depots.html',
  styleUrl: './depots.css'
})
export default class Depots {

  readonly newDepot = signal<DepotModel>({ ...initialDepot })
  readonly updateDepotValues = signal<DepotModel>({ ...initialDepot })
  readonly updateDepotId = signal<string>("")
  readonly depots = httpResource<ODataResponse<DepotModel>>(() => "http://localhost:5113/odata/depots")
  readonly loading = computed(() => this.depots.isLoading())

  @ViewChild('addFirstInput') addFirstInput!: ElementRef<HTMLInputElement>
  @ViewChild('updateFirstInput') updateFirstInput!: ElementRef<HTMLInputElement>
  @ViewChild('addModal') addModalRef!: ElementRef<HTMLDivElement>;
  @ViewChild('updateModal') updateModalRef!: ElementRef<HTMLDivElement>;

  readonly #toastr = inject(FlexiToastService)
  readonly #http = inject(Http)

  readonly data = computed(() => {

    return this.depots.value()?.value.map((val, i) => {

      return {
        ...val,
        fullAddress: `${val.city} ${val.town} ${val.street}`

      } as DepotModel

    }) ?? []

  })

  openAddModal() {

    this.newDepot.set({ ...initialDepot })

    const modalEl = this.addModalRef.nativeElement
    const modal = new bootstrap.Modal(modalEl)

    modalEl.addEventListener('shown.bs.modal', () => {
      this.addFirstInput?.nativeElement.focus()
    }, { once: true })

    modal.show()

  }

  openUpdateModal(id: string) {

    const modalEl = this.updateModalRef.nativeElement
    const modal = new bootstrap.Modal(modalEl)

    modalEl.addEventListener('shown.bs.modal', () => {
      this.updateFirstInput?.nativeElement.focus()
    }, { once: true })

    this.getValuesForUpdate(id)

    modal.show()

  }

  saveDepot(form: NgForm) {

    if (!form.valid) {

      this.#toastr.showToast("Missing Data", "There are empty fields!", "error")
      return

    }

    this.newDepot.set(form.value)

    this.#http.post<ResultModel<DepotModel>>("depot", this.newDepot(), (res) => {

      this.#toastr.showToast("Success", "Depot successfully created.", "success")

      const modalInstance = bootstrap.Modal.getInstance(this.addModalRef.nativeElement)
      modalInstance?.hide()

      this.depots.reload()

    })

  }

  updateDepot(form: NgForm) {

    this.#http.put<ResultModel<DepotModel>>("depot", this.updateDepotValues(), (res) => {

      this.#toastr.showToast("Success", "Depot successfully updated.", "success")

      const modalInstance = bootstrap.Modal.getInstance(this.updateModalRef.nativeElement)
      modalInstance?.hide()

      this.depots.reload()

    })

  }

  deleteDepot(id: string, fullName: string) {

    this.#toastr.showSwal("Delete Depot?", `Are you sure about deleting depot ${fullName}?`, "Delete", () => {

      this.#http.delete(`depot/${id}`, (res) => {

        if (res.isSuccessful) {

          this.#toastr.showToast("Success", `Depot(${fullName}) is deleted.`, "success");
          this.depots.reload();

        } else {

          this.#toastr.showToast("Error", `Depot(${fullName}) could not be deleted`, "error");

        }

      })

    }, "Cancel")

  }

  getValuesForUpdate(id: string) {

    const depot = this.depots.value()?.value.find(depot => depot.id === id)

    if (!depot) {

      this.#toastr.showToast("Problem", "Record might have been deleted", "error")
      return

    }

    this.updateDepotValues.set({ ...depot })
    this.updateDepotId.set(id)

  }

}