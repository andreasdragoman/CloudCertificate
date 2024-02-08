import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({providedIn: 'root'})
export class UserStoreService {

    private fullName$ = new BehaviorSubject<string>("");
    private role$ = new BehaviorSubject<string>("");
    private isUserLoggedIn$ = new BehaviorSubject<boolean>(false);

    constructor(){}

    public getRoleFromStore(){
        return this.role$.asObservable();
    }

    public setRoleForStore(role: string){
        this.role$.next(role);
    }

    public getFullNameFromStore(){
        return this.fullName$.asObservable();
    }

    public setFullNameForStore(fullName: string){
        this.fullName$.next(fullName);
    }

    public getIsUserLoggedInFromStore(){
        return this.isUserLoggedIn$.asObservable();
    }

    public setIsUserLoggedInForStore(isLoggedIn: boolean){
        this.isUserLoggedIn$.next(isLoggedIn);
    }
}