import { Component, inject, signal } from '@angular/core';
import { SharedModule } from '../../modules/shared-module';
import { initialLoginModel, LoginModel } from '../../models/login.model';
import { NgForm } from '@angular/forms';
import { Http } from '../../services/http';
import { LoginResponseModel } from '../../models/login-response.model';
import { Router } from '@angular/router';
import { FlexiToastService } from 'flexi-toast';

@Component({
  selector: 'app-login',
  imports: [
    SharedModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
})

export default class Login {

  readonly #http = inject(Http)
  readonly #router = inject(Router)
  readonly #toast = inject(FlexiToastService)

  readonly loginModel = signal<LoginModel>({ ...initialLoginModel })

  signIn(form: NgForm) {

    if (form.valid) {

      this.#http.post<LoginResponseModel>("Auth/Login", this.loginModel(), (res) => {

        if (res.data !== undefined) {
            
            localStorage.clear()
            localStorage.setItem("token", res.data?.accessToken)
            this.#router.navigateByUrl("/");

          }

      })

    }

  }

}
