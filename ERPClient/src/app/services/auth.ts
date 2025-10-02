import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { UserModel } from '../models/user.model';
import { TokenModel } from '../models/token.model';

@Injectable({
  providedIn: 'root'
})
export class Auth {

  readonly router = inject(Router)

  user: UserModel = new UserModel()
  readonly token = signal<string>(localStorage.getItem("token")!)
  
  isAuthenticated() {

    if(this.token() === "") {
      this.router.navigateByUrl("/")
      return false
    }

    if(this.token()) {

      const decode: JwtPayload | any = jwtDecode(this.token())

      this.user.id = decode["user-id"]
      this.user.name = decode["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]
      this.user.email = decode["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"]
      this.user.userName = decode["UserName"]

      const expires = decode.exp;
      const now = new Date().getTime() / 1000;

      if(now > expires) {

        localStorage.removeItem("token")
        this.router.navigateByUrl("/")
        return false

      }

      return true

    }

    this.router.navigateByUrl("/login")
    return false;

  }

}