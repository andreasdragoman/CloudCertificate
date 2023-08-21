import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class PersonService {
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

    getPersons(): Observable<string[]> {
        return this.http.get<string[]>(this.baseUrl + 'persons/get');//https://localhost:44395/persons/get
    }
}