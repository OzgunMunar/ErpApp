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
import { initialProduct, ProductModel } from '../../models/product.model';
import { productTypes } from '../../constants';
import { initialProductType, ProductTypeModel } from '../../models/product-type.model';

@Component({

  selector: 'app-products',
  imports: [
    Section,
    Blank,
    FlexiGridModule,
    FormsModule
  ],
  templateUrl: './products.html',
  styleUrl: './products.css'

})

export default class Products {

  readonly products = httpResource<{ value: ProductModel[] }>(() => "http://localhost:5113/odata/products")

  readonly productTypes = signal<ProductTypeModel[]>(productTypes)

  readonly newProduct = signal<ProductModel>({ ...initialProduct })
  readonly updateProductValues = signal<ProductModel>({ ...initialProduct })
  readonly updateProductId = signal<string>("")

  readonly loading = computed(() => this.products.isLoading())

  @ViewChild('addFirstInput') addFirstInput!: ElementRef<HTMLInputElement>
  @ViewChild('updateFirstInput') updateFirstInput!: ElementRef<HTMLInputElement>
  @ViewChild('addModal') addModalRef!: ElementRef<HTMLDivElement>;
  @ViewChild('updateModal') updateModalRef!: ElementRef<HTMLDivElement>;

  readonly #toastr = inject(FlexiToastService)
  readonly #http = inject(Http)

  readonly data = computed(() => 

    this.products.value()?.value.map((val) => ({

      ...val,
      productTypeName: productTypes.find(pt => pt.value == val.productType)?.name ?? ""

    })) ?? []

  );

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

      this.#toastr.showToast("Missing Data", "There are empty fields!", "error")
      return

    }

    this.newProduct.set(form.value)

    this.#http.post<ResultModel<ProductModel>>("product", this.newProduct(), (res) => {

      this.#toastr.showToast("Success", "Product successfully created.", "success")

      const modalInstance = bootstrap.Modal.getInstance(this.addModalRef.nativeElement)
      modalInstance?.hide()

      this.products.reload()

    })

  }

  updateProduct(form: NgForm) {

    this.#http.put<ResultModel<ProductModel>>("product", this.updateProductValues(), (res) => {

      this.#toastr.showToast("Success", "Product successfully updated.", "success")

      const modalInstance = bootstrap.Modal.getInstance(this.updateModalRef.nativeElement)
      modalInstance?.hide()

      this.products.reload()

    })

  }

  deleteProduct(id: string, fullName: string) {

    this.#toastr.showSwal("Delete Product?", `Are you sure about deleting product ${fullName}?`, "Delete", () => {

      this.#http.delete(`product/${id}`, (res) => {

        if (res.isSuccessful) {

          this.#toastr.showToast("Success", `Product(${fullName}) is deleted.`, "success");
          this.products.reload();

        } else {

          this.#toastr.showToast("Error", `Product(${fullName}) could not be deleted`, "error");

        }

      })

    }, "Cancel")

  }

  getValuesForUpdate(id: string) {

    const product = this.products.value()?.value.find(product => product.id === id);


    if (!product) {

      this.#toastr.showToast("Problem", "Record might have been deleted", "error")
      return

    }

    this.updateProductValues.set({ ...product })
    this.updateProductId.set(id)

  }

}