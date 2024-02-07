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
        this.finalBaseUrl += "/api";
    }

    getPersons(): Observable<Person[]> {
        return this.http.get<Person[]>(this.finalBaseUrl + '/persons');
    }

    addPerson(person: Person): Observable<any> {
        return this.http.post(this.finalBaseUrl + '/persons', person);
    }

    updatePerson(person: Person): Observable<any> {
        return this.http.put(this.finalBaseUrl + '/persons/' + person.id, person);
    }

    deletePerson(id: string): Observable<any> {
        return this.http.delete(this.finalBaseUrl + '/persons/' + id);
    }
}