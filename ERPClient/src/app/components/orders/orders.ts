import { Component, computed, effect, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { Section } from '../section/section';
import { Blank } from '../blank/blank';
import { httpResource } from '@angular/common/http';
import { FlexiGridModule } from 'flexi-grid';
import { FormsModule, NgForm } from '@angular/forms';
import { FlexiToastService } from 'flexi-toast';
import { Http } from '../../services/http';
import * as bootstrap from 'bootstrap';
import { ResultModel } from '../../models/result.model';
import { initialOrderModel, OrderModel } from '../../models/order.model';
import { CustomerModel } from '../../models/customer.model';
import { ProductModel } from '../../models/product.model';
import { initialOrderDetailModel, OrderDetailModel } from '../../models/order-detail.model';
import { DatePipe } from '@angular/common';
import { initialOrderStatusModel, OrderStatusModel } from '../../models/order-status.model';
import { orderStatus } from '../../constants';

@Component({

  selector: 'app-orders',
  imports: [
    Section,
    Blank,
    FlexiGridModule,
    FormsModule
  ],
  providers: [DatePipe],
  templateUrl: './orders.html',
  styleUrl: './orders.css'

})

export default class Orders {

  readonly orders = httpResource<ODataResponse<OrderModel>>(() => "http://localhost:5113/odata/orders")
  readonly customers = httpResource<ODataResponse<CustomerModel>>(() => "http://localhost:5113/odata/customers")
  readonly products = httpResource<ODataResponse<ProductModel>>(() => "http://localhost:5113/odata/products")

  readonly newOrder = signal<OrderModel>({ ...initialOrderModel })
  readonly updateOrderValues = signal<OrderModel>({ ...initialOrderModel })
  readonly detail = signal<OrderDetailModel>({ ...initialOrderDetailModel })
  readonly orderStatuses = signal<OrderStatusModel[]>(orderStatus)
  readonly updateOrderId = signal<string>("")

  readonly loading = computed(() => this.orders.isLoading())

  readonly datePipe = inject(DatePipe)

  @ViewChild('addModal') addModalRef!: ElementRef<HTMLDivElement>;
  @ViewChild('updateModal') updateModalRef!: ElementRef<HTMLDivElement>;

  readonly #toastr = inject(FlexiToastService)
  readonly #http = inject(Http)

  readonly data = computed(() => {

    const customersValue = this.customerData();

    return this.orders.value()?.value.map(order => {

      const customerInfo = customersValue.find(c => c.id === order.customerId);

      return {

        ...order,
        customer: customerInfo,
        orderedDate: this.datePipe.transform(order.orderedDate, 'dd/MM/yyyy'),
        deliveryDate: this.datePipe.transform(order.deliveryDate, 'dd/MM/yyyy'),
        customerFullAddress: customerInfo ? `${customerInfo.city} ${customerInfo.town} ${customerInfo.street}` : '',
        statusName: orderStatus.find(s => s.value === order.status)?.name ?? ''

      } as OrderModel

    }) ?? []

  })

  readonly customerData = computed<CustomerModel[]>(() => this.customers.value()?.value ?? [])
  readonly productData = computed<ProductModel[]>(() => this.products.value()?.value ?? [])

  openAddModal() {

    this.newOrder.set({ ...initialOrderModel })

    const modalEl = this.addModalRef.nativeElement
    const modal = new bootstrap.Modal(modalEl)

    modal.show()

  }

  openUpdateModal(id: string) {

    const modalEl = this.updateModalRef.nativeElement
    const modal = new bootstrap.Modal(modalEl)

    this.getValuesForUpdate(id)

    modal.show()

  }

  addDetail(operation: string) {

    const relatedProduct = this.products.value()?.value.find(p => p.id == this.detail().productId)

    if (relatedProduct) {

      const newDetail = { ...this.detail(), product: relatedProduct }
      this.detail.set(newDetail)

    }

    if(operation === "create") {

      this.newOrder().details.push(this.detail())

    } else {

      this.updateOrderValues().details.push(this.detail())
      
    }

    this.detail.set({ ...initialOrderDetailModel })

  }

  removeDetail(index: number, operation: string) {

    if (operation === "create") {

      this.newOrder.update(val => ({

        ...val,
        details: val.details.filter((_, i) => i !== index)

      }))

    } else {

      this.updateOrderValues.update(val => ({

        ...val,
        details: val.details.filter((_, i) => i !== index)

      }))

    }

  }

  saveOrder(form: NgForm) {

    if (!form.valid) {

      this.#toastr.showToast("Missing Data", "There are empty fields!", "error")
      return

    }

    this.#http.post<ResultModel<OrderModel>>("order", this.newOrder(), (res) => {

      this.#toastr.showToast("Success", "Order successfully created.", "success")

      const modalInstance = bootstrap.Modal.getInstance(this.addModalRef.nativeElement)
      modalInstance?.hide()

      this.orders.reload()

    })

  }

  updateOrder(form: NgForm) {

    // console.table(this.updateOrderValues())
    // return

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