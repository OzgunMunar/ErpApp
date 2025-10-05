import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ResultModel } from '../models/result.model';
import { api } from '../constants';
import { ErrorService } from './error';
import { Auth } from './auth';

@Injectable({
  providedIn: 'root'
})
export class Http {
  
  readonly http = inject(HttpClient)
  readonly errorService = inject(ErrorService)
  readonly auth = inject(Auth)

  getById<T>(
    apiUrl: string,
    parameterName: string,
    id: number | string,
    callBack: (res: ResultModel<T>) => void,
    errorCallBack?: (err: HttpErrorResponse) => void
  ) {

    let headers = new HttpHeaders();
    
    headers = headers.set('Authorization', `Bearer ${this.auth.token}`);

    this.http.get<ResultModel<T>>(`${api}/${apiUrl}?${parameterName}=${id}`, { headers })
      .subscribe({
        next: (res) => {
          
          callBack(res);
          
        },
        error: (err: HttpErrorResponse) => {
          this.errorService.errorHandler(err);
          if (errorCallBack !== undefined) {
            errorCallBack(err);
          }
        }
      });
  }

   getByIdRouteParam<T>(
    apiUrl: string,
    id: number | string,
    callBack: (res: ResultModel<T>) => void,
    errorCallBack?: (err: HttpErrorResponse) => void
  ) {

    let headers = new HttpHeaders();
    
    headers = headers.set('Authorization', `Bearer ${this.auth.token}`);

    this.http.get<ResultModel<T>>(`${api}/${apiUrl}/${id}`, { headers })
      .subscribe({
        next: (res) => {
          
          callBack(res);
          
        },
        error: (err: HttpErrorResponse) => {
          this.errorService.errorHandler(err);
          if (errorCallBack !== undefined) {
            errorCallBack(err);
          }
        }
      });
  }

  post<T>(
    apiUrl: string,
    body: any,
    callBack: (res: ResultModel<T>) => void,
    errorCallBack?: (err: HttpErrorResponse) => void
  ) {

    let headers = new HttpHeaders()

    headers = headers.set('Authorization', `Bearer ${this.auth.token}`);

    this.http.post<ResultModel<T>>(`${api}/${apiUrl}`, body, { headers })
      .subscribe({
        next: (res => {
            callBack(res)
        }),
        error: ((err: HttpErrorResponse) => {

          this.errorService.errorHandler(err)

          if (errorCallBack !== undefined) {

            errorCallBack(err)

          }

        })

      })

  }

  delete<T>(
    apiUrl: string,
    callBack: (res: ResultModel<T>) => void,
    errorCallBack?: (err: HttpErrorResponse) => void
  ) {

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.auth.token}`);

    this.http.delete<ResultModel<T>>(`${api}/${apiUrl}`, { headers })
      .subscribe({
        next: (res => {

          callBack(res)

        }),
        error: ((err: HttpErrorResponse) => {

          this.errorService.errorHandler(err)
          if (errorCallBack !== undefined) {

            errorCallBack(err)

          }

        })

      })

  }

  put<T>(
    apiUrl: string,
    body: any,
    callBack: (res: ResultModel<T>) => void,
    errorCallBack?: (err: HttpErrorResponse) => void
  ) {

    let headers = new HttpHeaders()
    headers = headers.set('Authorization', `Bearer ${this.auth.token}`);

    this.http.put<ResultModel<T>>(`${api}/${apiUrl}`, body, { headers })
      .subscribe({
        next: (res => {

          callBack(res)

        }),
        error: ((err: HttpErrorResponse) => {

          this.errorService.errorHandler(err)

          if (errorCallBack !== undefined) {

            errorCallBack(err)

          }

        })

      })

  }

}
