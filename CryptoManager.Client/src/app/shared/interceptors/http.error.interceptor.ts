import { Observable, BehaviorSubject, throwError } from "rxjs";
import { Injectable } from "@angular/core";
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
} from "@angular/common/http";

import { AccountService } from "../services/index";
import { Router } from "@angular/router";
import { catchError, finalize, map } from "rxjs/operators";

@Injectable()
export class HTTPStatus {
  private requestInFlight$: BehaviorSubject<boolean>;
  constructor() {
    this.requestInFlight$ = new BehaviorSubject(false);
  }

  setHttpStatus(inFlight: boolean) {
    this.requestInFlight$.next(inFlight);
  }

  getHttpStatus(): Observable<boolean> {
    return this.requestInFlight$.asObservable();
  }
}

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  constructor(
    private accountService: AccountService,
    private router: Router,
    private status: HTTPStatus
  ) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (!req.urlWithParams.endsWith("/ping")) {
      this.status.setHttpStatus(true);
    }
    return next.handle(req).pipe(
      map((event) => {
        return event;
      }),
      catchError((error) => {
        //intercept the respons error and displace it to the console
        console.log("Error Occurred");
        console.log(error);
        //if ocurred 401 error, token should be expired, then user will login again
        if (error.status == "401") {
          this.accountService.purgeAuth();
        }
        //return the error to the method that called it
        return throwError(() => new Error(error));
      }),
      finalize(() => {
        if (!req.urlWithParams.endsWith("/ping")) {
          this.status.setHttpStatus(false);
        }
      })
    );
  }
}
