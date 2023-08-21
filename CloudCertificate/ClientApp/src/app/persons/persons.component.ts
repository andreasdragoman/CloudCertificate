import { Component, OnInit } from "@angular/core";
import { PersonService } from "../services/persons.service";

@Component({
    selector:'persons-component',
    templateUrl: './persons.component.html'
})
export class PersonsComponent implements OnInit {

    public personsList: string[] = [];

    constructor (private personService: PersonService) {
    }

    ngOnInit(): void {
        this.personService.getPersons().subscribe(result => {
            this.personsList = result;
        });
    }

}