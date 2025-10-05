import { Component, computed, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { httpResource } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FlexiToastService } from 'flexi-toast';
import { Http } from '../../services/http';
import { initialRecipe, RecipeModel } from '../../models/recipe.model';
import { initialRecipeDetailModel, RecipeDetailModel } from '../../models/recipe-detail.model';
import { ProductModel } from '../../models/product.model';
import { ResultModel } from '../../models/result.model';
import { Blank } from '../blank/blank';
import { Section } from '../section/section';
import { FlexiGridModule } from 'flexi-grid';
import * as bootstrap from 'bootstrap';

@Component({

  selector: 'app-recipedetails',
  templateUrl: './recipe-details.html',
  styleUrl: './recipe-details.css',
  imports: [
    Blank,
    Section,
    FlexiGridModule,
    FormsModule
  ]

})

export default class RecipeDetails {

  constructor(private activated: ActivatedRoute) {

    this.activated.params.subscribe(res => {

      const id = res['id']

      if (!id) return

      this.recipeId.set(id)

      this.getRecipeById(this.recipeId())

    })

  }

  readonly recipe = signal<RecipeModel>({ ...initialRecipe })
  readonly products = httpResource<{ value: ProductModel[] }>(() => "http://localhost:5113/odata/products")
  readonly productList = computed<ProductModel[]>(() => this.products.value()?.value ?? [])
  readonly recipeId = signal<string>("");

  readonly recipeDetails = computed(() => this.recipe().details ?? [{ ...initialRecipeDetailModel }])

  readonly newRecipeDetail = signal<RecipeDetailModel>({ ...initialRecipeDetailModel })
  readonly updateRecipeDetail = signal<RecipeDetailModel>({ ...initialRecipeDetailModel })

  readonly #http = inject(Http)
  readonly #toast = inject(FlexiToastService)

  getRecipeById(id: string) {

    this.#http.getByIdRouteParam<RecipeModel>("recipedetails", id, (res) => {

      this.recipe.set(res.data ?? { ...initialRecipe })

    })

  }

  saveRecipeDetail(form: NgForm) {

    if (!form.valid) {

      this.#toast.showToast("Missing Data", "There are empty fields!", "error")
      return

    }

    this.newRecipeDetail.set(form.value);

    this.#http.post<ResultModel<RecipeDetailModel>>("recipedetail", this.newRecipeDetail(), (res) => {

      this.#toast.showToast("Success", "Recipe Detail successfully created.", "success")
      this.newRecipeDetail.set({ ...initialRecipeDetailModel })
      this.getRecipeById(this.recipeId())

    })

  }

  deleteRecipeDetail(detail: RecipeDetailModel) {

    this.#toast.showSwal(
      "Delete Recipe Detail?",
      `Are you sure about deleting Recipe Detail ${detail.product?.productName}?`,
      "Delete",
      () => {

        this.#http.delete(`recipedetails/${detail.id}`, (res) => {

          if (res.isSuccessful) {

            this.#toast.showToast("Success", `Recipe Detail (${detail.product?.productName}) is deleted.`, "success")

            this.getRecipeById(this.recipeId())

          } else {

            this.#toast.showToast("Error", `Recipe Detail (${detail.product?.productName}) could not be deleted.`, "error")

          }

        })

      },
      "Cancel"
    )
  }

}
