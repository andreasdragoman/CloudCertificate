import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Person } from "../shared/models/person.model";
import { isDevMode } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class PersonService {
    finalBaseUrl?: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
        if(isDevMode()){
            this.finalBaseUrl = "https://localhost:44395";
        }
        else{
            this.finalBaseUrl = this.baseUrl;
        }
    }

    getPersons(): Observable<Person[]> {
        return this.http.get<Person[]>(this.finalBaseUrl + '/persons/get');
    }

    addPerson(person: Person): Observable<any> {
        return this.http.post(this.finalBaseUrl + '/persons/add', person);
    }
}