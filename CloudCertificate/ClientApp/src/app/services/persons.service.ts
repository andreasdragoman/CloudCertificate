import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Person } from "../shared/models/person.model";

@Injectable({ providedIn: 'root' })
export class PersonService {
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

    getPersons(): Observable<string[]> {
        return this.http.get<string[]>('https://localhost:44395/persons/get');//this.baseUrl + 'persons/get'
    }

    addPerson(person: Person): Observable<any> {
        return this.http.post('https://localhost:44395/persons/add', person);//this.baseUrl + 'persons/get'
    }
}