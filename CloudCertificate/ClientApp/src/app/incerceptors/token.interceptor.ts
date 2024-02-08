import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, catchError, throwError } from "rxjs";
import { AccountService } from "../services/account.service";
import { ToastrService } from "ngx-toastr";
import { Router } from "@angular/router";


@Injectable()
export class TokenInterceptor implements HttpInterceptor{

    constructor(private accountService: AccountService, private toastr: ToastrService, private router: Router) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const myToken = this.accountService.getAuthToken();

        if (myToken) {
            req = req.clone({
                setHeaders: {Authorization: `Bearer ${myToken}`}
            });
        }

        return next.handle(req).pipe(
            catchError((err: any) => {
                if(err instanceof HttpErrorResponse){
                    if(err.status === 401) {
                        this.toastr.warning('You should login again. Token expired');
                        this.router.navigate(['login']);
                    }
                }
                return throwError(() => new Error("Some other error occured"));
            })
        );
    }
}