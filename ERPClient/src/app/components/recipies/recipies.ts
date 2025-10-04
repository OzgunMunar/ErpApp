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
import { ProductModel } from '../../models/product.model';
import { initialRecipe, RecipeModel } from '../../models/recipe.model';
import { initialRecipeDetailModel, RecipeDetailModel } from '../../models/recipe-detail.model';

@Component({

  selector: 'app-recipies',
  imports: [
    Section,
    Blank,
    FlexiGridModule,
    FormsModule
  ],
  templateUrl: './recipies.html',
  styleUrl: './recipies.css'

})

export default class Recipies {

  readonly recipies = httpResource<RecipeModel[]>(() => "http://localhost:5113/odata/recipies")
  readonly products = httpResource<ProductModel[]>(() => "http://localhost:5113/odata/products")

  readonly data = computed<RecipeModel[]>(() => this.recipies.value() ?? [])
  readonly productsList = computed<ProductModel[]>(() => this.products.value() ?? [])
  readonly semiFinishedProductList = computed<ProductModel[]>(() => 
    
    (this.products.value() ?? []).filter(prod => prod.productType == 2)
    
  )
  readonly finishedProductList = computed<ProductModel[]>(() => 
    
    (this.products.value() ?? []).filter(prod => prod.productType == 1)
    
  )

  readonly productId = signal<string>("")
  readonly quantity = signal<number>(0)
  readonly detail = signal<RecipeDetailModel>({ ...initialRecipeDetailModel })

  readonly newRecipe = signal<RecipeModel>({ ...initialRecipe })
  readonly loading = computed(() => this.recipies.isLoading())

  @ViewChild('addFirstInput') addFirstInput!: ElementRef<HTMLInputElement>
  @ViewChild('addModal') addModalRef!: ElementRef<HTMLDivElement>

  readonly #toastr = inject(FlexiToastService)
  readonly #http = inject(Http)

  constructor() {

    effect(() => {

      // console.log("recipies:", this.recipies.value());
      // console.log("data:", this.data());

    })

  }

  openAddModal() {

    this.newRecipe.set({ ...initialRecipe })

    const modalEl = this.addModalRef.nativeElement
    const modal = new bootstrap.Modal(modalEl)

    modalEl.addEventListener('shown.bs.modal', () => {
      this.addFirstInput?.nativeElement.focus()
    }, { once: true })

    modal.show()

  }

  saveRecipe(form: NgForm) {

    if (!form.valid) {

      this.#toastr.showToast("Missing Data", "There are empty fields!", "error")
      return

    }

    this.newRecipe.update(val => ({

      ...val,
      ...form.value

    }))

    if (this.newRecipe().details.length === 0) {

      this.#toastr.showToast("Missing Data", "Please add at least one detail!", "error")
      return

    }

    this.#http.post<ResultModel<ProductModel>>("recipies", this.newRecipe(), (res) => {

      this.#toastr.showToast("Success", "Recipe successfully created.", "success")

      const modalInstance = bootstrap.Modal.getInstance(this.addModalRef.nativeElement)
      modalInstance?.hide()

      this.recipies.reload()

    })

  }

  deleteRecipe(id: string, recipeName: string) {

    this.#toastr.showSwal("Delete Recipe?", `Are you sure about deleting recipe ${recipeName}?`, "Delete", () => {

      this.#http.delete(`recipies/${id}`, (res) => {

        if (res.isSuccessful) {

          this.#toastr.showToast("Success", `Recipe(${recipeName}) is deleted.`, "success");
          this.recipies.reload();

        } else {

          this.#toastr.showToast("Error", `recipe(${recipeName}) could not be deleted`, "error");

        }

      })

    }, "Cancel")

  }

  addDetail() {

    const relatedProduct = this.productsList().find(p => p.id == this.detail().productId)

    if (relatedProduct) {

      this.detail.update(val => ({

        ...val,
        product: relatedProduct

      }))

    }

    this.newRecipe.update(val => ({

      ...val,
      details: [...val.details, this.detail()]

    }))

    this.detail.set({ ...initialRecipeDetailModel })

  }

  removeDetail(index: number) {

    this.newRecipe.update(val => ({

      ...val,
      details: val.details.filter((_, i) => i !== index)

    }))

  }

}