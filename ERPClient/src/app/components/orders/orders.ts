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
import { initialOrderModel, OrderModel } from '../../models/order.model';

@Component({
  selector: 'app-orders',
  imports: [
    Section,
    Blank,
    FlexiGridModule,
    FormsModule
  ],
  templateUrl: './orders.html',
  styleUrl: './orders.css'
})
export default class Orders {

  readonly newOrder = signal<OrderModel>({ ...initialOrderModel })
  readonly updateOrderValues = signal<OrderModel>({ ...initialOrderModel })
  readonly updateOrderId = signal<string>("")
  readonly orders = httpResource<ODataResponse<OrderModel>>(() => "http://localhost:5113/odata/orders")
  readonly loading = computed(() => this.orders.isLoading())

  @ViewChild('addFirstInput') addFirstInput!: ElementRef<HTMLInputElement>
  @ViewChild('updateFirstInput') updateFirstInput!: ElementRef<HTMLInputElement>
  @ViewChild('addModal') addModalRef!: ElementRef<HTMLDivElement>;
  @ViewChild('updateModal') updateModalRef!: ElementRef<HTMLDivElement>;

  readonly #toastr = inject(FlexiToastService)
  readonly #http = inject(Http)

  readonly data = computed(() => {

    return this.orders.value()?.value.map((val, i) => {

      return {
        ...val,
        customerFullAddress: `${val.customer.city} ${val.customer.town} ${val.customer.street}`

      } as OrderModel

    }) ?? []

  })

  openAddModal() {

    this.newOrder.set({ ...initialOrderModel })

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

  saveOrder(form: NgForm) {

    if (!form.valid) {

      this.#toastr.showToast("Missing Data", "There are empty fields!", "error")
      return

    }

    this.newOrder.set(form.value)

    this.#http.post<ResultModel<OrderModel>>("order", this.newOrder(), (res) => {

      this.#toastr.showToast("Success", "Order successfully created.", "success")

      const modalInstance = bootstrap.Modal.getInstance(this.addModalRef.nativeElement)
      modalInstance?.hide()

      this.orders.reload()

    })

  }

  updateOrder(form: NgForm) {

    this.#http.put<ResultModel<OrderModel>>("order", this.updateOrderValues(), (res) => {

      this.#toastr.showToast("Success", "Order successfully updated.", "success")

      const modalInstance = bootstrap.Modal.getInstance(this.updateModalRef.nativeElement)
      modalInstance?.hide()

      this.orders.reload()

    })

  }

  deleteOrder(order: OrderModel) {

    this.#toastr.showSwal("Delete Order?", `Are you sure about deleting order ${order.orderNumber}?`, "Delete", () => {

      this.#http.delete(`order/${order.id}`, (res) => {

        if (res.isSuccessful) {

          this.#toastr.showToast("Success", `Order(${order.orderNumber}) is deleted.`, "success");
          this.orders.reload();

        } else {

          this.#toastr.showToast("Error", `Depot(${order.orderNumber}) could not be deleted`, "error");

        }

      })

    }, "Cancel")

  }

  getValuesForUpdate(id: string) {

    const order = this.orders.value()?.value.find(order => order.id === id)

    if (!order) {

      this.#toastr.showToast("Problem", "Record might have been deleted", "error")
      return

    }

    this.updateOrderValues.set({ ...order })
    this.updateOrderId.set(id)

  }

}