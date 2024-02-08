import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { AccountService } from "../services/account.service";
import { ToastrService } from "ngx-toastr";

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {

    constructor(private accountService: AccountService, private router: Router, private toastr: ToastrService) { 
    }

    canActivate(): boolean {
        if (this.accountService.isLoggedIn()){
            return true;
        }
        else{
            this.toastr.warning('Please login first', 'Warning');
            this.router.navigate(['login']);
            return false;
        }
    }
}