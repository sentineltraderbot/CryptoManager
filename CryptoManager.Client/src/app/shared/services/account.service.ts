import { Injectable } from "@angular/core";
import { Observable, BehaviorSubject, ReplaySubject } from "rxjs";

import { ApiService } from "./api.service";
import { JwtService } from "./jwt.service";
import { UserToken } from "../models/user-token.model";
import { distinctUntilChanged, map } from "rxjs/operators";
import { JwtHelper } from "../helpers/index";
import { HttpParams } from "@angular/common/http";
import { Router } from "@angular/router";
import { ApiType } from "../models/api-type.enum";
import { UserDetails } from "../models/user-details.model";
declare const FB: any;

@Injectable()
export class AccountService {
  public userFields: string[] = ["email", "id", "name", "picture"];
  public fbApiPermission: string[] = ["email"];

  public serviceURL = "/account";
  private currentUserSubject = new BehaviorSubject<UserToken>({} as UserToken);
  public currentUser = this.currentUserSubject
    .asObservable()
    .pipe(distinctUntilChanged());

  private isAuthenticatedSubject = new ReplaySubject<boolean>(1);
  public isAuthenticated = this.isAuthenticatedSubject.asObservable();

  constructor(
    private apiService: ApiService,
    private jwtService: JwtService,
    private jwtHelper: JwtHelper,
    private router: Router
  ) {}

  populate() {
    if (this.jwtService.getToken()) {
      if (this.currentUserSubject.value.toString() == "[object Object]") {
        let user = this.createUserModel(this.jwtService.getToken());
        this.setAuth(user);
      }
    } else if (localStorage.getItem("isLoggedin") == "true") {
      this.purgeAuth();
    }
  }

  setAuth(user: UserToken) {
    this.jwtService.saveToken(user.token);
    this.currentUserSubject.next(user);
    this.isAuthenticatedSubject.next(true);
    localStorage.setItem("isLoggedin", "true");
  }

  purgeAuth() {
    this.jwtService.destroyToken();
    this.currentUserSubject.next({} as UserToken);
    this.isAuthenticatedSubject.next(false);
    localStorage.setItem("isLoggedin", "false");
    if (this.router.url != "/login") {
      this.router.navigate(["login"]);
    }
  }

  facebookLogin(): Observable<boolean> {
    return new Observable<boolean>((observer) => {
      FB.getLoginStatus((response) => {
        if (response.status === "connected") {
          return this.authFacebookUser(
            response.authResponse.accessToken
          ).subscribe({
            next: (data) => {
              observer.next(data);
            },
            error: (error) => {
              observer.error(error);
              this.purgeAuth();
            },
          });
        } else {
          FB.login(
            (loginResponse) => {
              if (loginResponse.status !== "not_authorized") {
                return this.authFacebookUser(
                  loginResponse.authResponse.accessToken
                ).subscribe({
                  next: (data) => {
                    observer.next(data);
                  },
                  error: (error) => {
                    observer.error(error);
                    this.purgeAuth();
                  },
                });
              } else {
                this.purgeAuth();
                observer.error("User Not logged");
              }
            },
            { scope: this.fbApiPermission.join(",") }
          );
        }
      });
    });
  }

  googleLogin(token: string): Observable<boolean> {
    return new Observable<boolean>((observer) => {
      return this.authGoogleUser(token).subscribe({
        next: (data) => {
          observer.next(data);
        },
        error: (error) => {
          observer.error(error);
          this.purgeAuth();
        },
      });
    });
  }

  authFacebookUser(accessToken: string): Observable<boolean> {
    let httpParams = new HttpParams().append("accessToken", accessToken);
    return this.apiService
      .post(
        this.serviceURL + "/ExternalLoginFacebook",
        null,
        httpParams,
        ApiType.CryptoManagerServerApi
      )
      .pipe(
        map((data) => {
          let user = this.createUserModel(data);
          this.setAuth(user);
          return true;
        })
      );
  }

  authGoogleUser(accessToken: string): Observable<boolean> {
    let httpParams = new HttpParams().append("token", accessToken);
    return this.apiService
      .post(
        this.serviceURL + "/ExternalLoginGoogle",
        null,
        httpParams,
        ApiType.CryptoManagerServerApi
      )
      .pipe(
        map((data) => {
          let user = this.createUserModel(data);
          this.setAuth(user);
          return true;
        })
      );
  }

  getUserDetails(): Observable<UserDetails> {
    return this.apiService
      .get(this.serviceURL, null, ApiType.CryptoManagerServerApi)
      .pipe(
        map((data: UserDetails) => {
          return data;
        })
      );
  }

  getCurrentUser(): UserToken {
    if (this.currentUserSubject.value) {
      return this.currentUserSubject.value;
    } else {
      this.purgeAuth();
    }
  }

  createUserModel(data: string): UserToken {
    let tokenDecoded = this.jwtHelper.decodeToken(data);
    let user: UserToken = new UserToken(tokenDecoded, data);
    return user;
  }
}
