import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { isDevMode } from "@angular/core";
import { LoginModel } from "../models/authentication/login.model";
import { RegisterModel } from "../models/authentication/register.model";

@Injectable({ providedIn: 'root' })
export class AccountService {
    finalBaseUrl?: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
        if(isDevMode()){
            this.finalBaseUrl = "http://localhost:44395";
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

}