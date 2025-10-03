import { httpResource } from '@angular/common/http';
import { Component, computed, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { initialProduct, ProductModel } from '../../models/product.model';
import { ProductTypeModel } from '../../models/product-type.model';
import { productTypes } from '../../constants';
import { FlexiToastService } from 'flexi-toast';
import { Http } from '../../services/http';
import bootstrap from 'bootstrap';
import { NgForm } from '@angular/forms';
import { ResultModel } from '../../models/result.model';

@Component({
  selector: 'app-products',
  imports: [],
  templateUrl: './products.html',
  styleUrl: './products.css'
})

export default class Products {

  readonly products = httpResource<ODataResponse<ProductModel>>(() => "http://localhost:5113/odata/products")
  readonly productTypeList = signal<ProductTypeModel[]>(productTypes)
  readonly newProduct = signal<ProductModel>({ ...initialProduct })
  readonly updateProductValues = signal<ProductModel>({ ...initialProduct })
  readonly updateProductId = signal<string>("")

  @ViewChild('addFirstInput') addFirstInput!: ElementRef<HTMLInputElement>
  @ViewChild('updateFirstInput') updateFirstInput!: ElementRef<HTMLInputElement>
  @ViewChild('addModal') addModalRef!: ElementRef<HTMLDivElement>
  @ViewChild('updateModal') updateModalRef!: ElementRef<HTMLDivElement>

  readonly #toast = inject(FlexiToastService)
  readonly #http = inject(Http)

  readonly data = computed(() => this.products.value() ?? [])

  openAddModal() {

    this.newProduct.set({ ...initialProduct })

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

  saveProduct(form: NgForm) {

    if (!form.valid) {

      this.#toast.showToast("Missing Data", "There are empty fields!", "error")
      return

    }

    this.newProduct.set(form.value)

    this.#http.post<ResultModel<ProductModel>>('product', this.newProduct(), () => {

      this.#toast.showToast("Success", "Product successfully created.", "success")

      const modalInstance = bootstrap.Modal.getInstance(this.addModalRef.nativeElement)
      modalInstance?.hide()

      this.products.reload()

    })

  }

  updateProduct(form: NgForm) {

    this.#http.put<ResultModel<ProductModel>>("product", this.updateProductValues(), (res) => {

      this.#toast.showToast("Success", "Product successfully updated.", "success")

      const modalInstance = bootstrap.Modal.getInstance(this.updateModalRef.nativeElement)
      modalInstance?.hide()

      this.products.reload()

    })

  }

  deleteProduct(id: string, productName: string) {

    this.#toast.showSwal("Delete Depot?", `Are you sure about deleting product ${productName}?`, "Delete", () => {

      this.#http.delete(`product/${id}`, (res) => {

        if (res.isSuccessful) {

          this.#toast.showToast("Success", `Product(${productName}) is deleted.`, "success");
          this.products.reload();

        } else {

          this.#toast.showToast("Error", `Product(${productName}) could not be deleted`, "error");

        }

      })

    }, "Cancel")

  }

  getValuesForUpdate(id: string) {

    const product = this.products.value()?.value.find(product => product.id === id)

    if (!product) {

      this.#toast.showToast("Problem", "Record might have been deleted", "error")
      return

    }

    this.updateProductValues.set({ ...product })
    this.updateProductId.set(id)

  }

}
// "SqlServer": "Server=localhost;Database=ERPDB;User Id=sa;Password=admin;TrustServerCertificate=True"