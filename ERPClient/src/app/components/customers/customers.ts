import { Component, computed, effect, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { Section } from '../section/section';
import { Blank } from '../blank/blank';
import { CustomerModel, initialCustomer } from '../../models/customer.model';
import { httpResource } from '@angular/common/http';
import { FlexiGridModule } from 'flexi-grid';
import { FormsModule, NgForm } from '@angular/forms';
import { FlexiToastService } from 'flexi-toast';
import { Http } from '../../services/http';
import * as bootstrap from 'bootstrap';
import { ResultModel } from '../../models/result.model';

@Component({
  selector: 'app-customers',
  imports: [
    Section,
    Blank,
    FlexiGridModule,
    FormsModule
  ],
  templateUrl: './customers.html',
  styleUrl: './customers.css'
})
export default class Customers {

  constructor() {

    effect(() => {

      // console.log(this.customers())

    })

  }

  readonly newCustomer = signal<CustomerModel>({ ...initialCustomer })
  readonly updateCustomerValues = signal<CustomerModel>({ ...initialCustomer })
  readonly updateCustomerId = signal<string>("")
  readonly customers = httpResource<ODataResponse<CustomerModel>>(() => "http://localhost:5113/odata/customers")
  readonly loading = computed(() => this.customers.isLoading())

  @ViewChild('addFirstInput') addFirstInput!: ElementRef<HTMLInputElement>
  @ViewChild('updateFirstInput') updateFirstInput!: ElementRef<HTMLInputElement>
  @ViewChild('addModal') addModalRef!: ElementRef<HTMLDivElement>;
  @ViewChild('updateModal') updateModalRef!: ElementRef<HTMLDivElement>;

  readonly #toastr = inject(FlexiToastService)
  readonly #http = inject(Http)

  readonly data = computed(() => {

    return this.customers.value()?.value.map((val, i) => {

      return {
        ...val,
        fullAddress: `${val.city} ${val.town} ${val.street}`

      } as CustomerModel

    }) ?? []

  })

  openAddModal() {

    this.newCustomer.set({ ...initialCustomer })

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

  saveCustomer(form: NgForm) {

    if (!form.valid) {

      this.#toastr.showToast("Missing Data", "There are empty fields!", "error")
      return

    }

    this.newCustomer.set(form.value)

    this.#http.post<ResultModel<CustomerModel>>("customer", this.newCustomer(), (res) => {

      this.#toastr.showToast("Success", "Customer successfully created.", "success")

      const modalInstance = bootstrap.Modal.getInstance(this.addModalRef.nativeElement)
      modalInstance?.hide()

      this.customers.reload()

    })

  }

  updateCustomer(form: NgForm) {

    this.#http.put<ResultModel<CustomerModel>>("customer", this.updateCustomerValues(), (res) => {

      this.#toastr.showToast("Success", "Customer successfully updated.", "success")

      const modalInstance = bootstrap.Modal.getInstance(this.updateModalRef.nativeElement)
      modalInstance?.hide()

      this.customers.reload()

    })

  }

  deleteCustomer(id: string, fullName: string) {

    this.#toastr.showSwal("Delete Customer?", `Are you sure about deleting customer ${fullName}?`, "Delete", () => {

      this.#http.delete(`customer/${id}`, (res) => {

        if (res.isSuccessful) {

          this.#toastr.showToast("Success", `Customer(${fullName}) is deleted.`, "success");
          this.customers.reload();

        } else {

          this.#toastr.showToast("Error", `Customer(${fullName}) could not be deleted`, "error");

        }

      })

    }, "Cancel")

  }

  getValuesForUpdate(id: string) {

    const customer = this.customers.value()?.value.find(customer => customer.id === id)

    if (!customer) {

      this.#toastr.showToast("Problem", "Record might have been deleted", "error")
      return

    }

    this.updateCustomerValues.set({ ...customer })
    this.updateCustomerId.set(id)

  }

}