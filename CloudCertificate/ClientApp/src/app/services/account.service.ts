import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { isDevMode } from "@angular/core";
import { LoginModel } from "../models/authentication/login.model";
import { RegisterModel } from "../models/authentication/register.model";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserStoreService } from "./user-store.service";

@Injectable({ providedIn: 'root' })
export class AccountService {
    finalBaseUrl?: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router, 
        private toastr: ToastrService, private userStore: UserStoreService) {
        if(isDevMode()){
            this.finalBaseUrl = "https://localhost:44395";
        }
        else{
            this.finalBaseUrl = this.baseUrl;
        }
        this.finalBaseUrl += "/api";
    }

    login(loginModel: LoginModel): Observable<any> {
        return this.http.post(this.finalBaseUrl + '/account/login', loginModel);
    }

    register(registerModel: RegisterModel): Observable<any> {
        return this.http.post(this.finalBaseUrl + '/account/register', registerModel);
    }

    logout() {
        this.http.post(this.finalBaseUrl + '/account/logout', null).subscribe({
            next: (res) => {
                this.toastr.success('Successfully logged out');
                localStorage.removeItem('authToken');
                this.userStore.setFullNameForStore("");
                this.userStore.setRoleForStore("");
                this.userStore.setIsUserLoggedInForStore(false);
                this.router.navigate(['']);
            },
            error: (res) => {
                this.toastr.error('Unable to logout');
            }
        });
        
    }

    storeAuthToken(tokenValue: string) {
        localStorage.setItem('authToken', tokenValue);
    }

    getAuthToken() {
        return localStorage.getItem('authToken');
    }

    isLoggedIn(): boolean {
        return !!localStorage.getItem('authToken');
    }

    getDecodedToken() {
        const jwtHelper = new JwtHelperService();
        const token = this.getAuthToken()!;
        return jwtHelper.decodeToken(token);
    }

    getFullNameFromToken() {
        if (this.getDecodedToken()) {
            return this.getDecodedToken().given_name;
        }
        return "";
    }

    getRoleFromToken() {
        if (this.getDecodedToken()) {
            return this.getDecodedToken().role;
        }
        return "";
    }
}