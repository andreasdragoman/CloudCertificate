import { Component, OnInit } from "@angular/core";
import { PersonService } from "../services/persons.service";
import { Person } from "../shared/models/person.model";
import { FormsModule } from "@angular/forms";

@Component({
    selector:'persons-component',
    templateUrl: './persons.component.html'
})
export class PersonsComponent implements OnInit {

    public personsList: string[] = [];

    personModel = new Person();
    submitted = false;

    constructor (private personService: PersonService) {
    }

    ngOnInit(): void {
        this.personService.getPersons().subscribe(result => {
            this.personsList = result;
        });
    }

    onSubmit() { 
        this.submitted = true;
        this.personService.addPerson(this.personModel).subscribe(result => {
            console.log(result);
        },
        () => {
            alert('Server error');
        });
    }

}