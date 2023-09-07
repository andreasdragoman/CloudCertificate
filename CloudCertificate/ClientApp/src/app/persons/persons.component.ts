import { Component, OnInit } from "@angular/core";
import { PersonService } from "../services/persons.service";
import { Person } from "../shared/models/person.model";
import { FormsModule } from "@angular/forms";
import { NotifierService } from "angular-notifier/lib/services/notifier.service";

@Component({
    selector:'persons-component',
    templateUrl: './persons.component.html'
})
export class PersonsComponent implements OnInit {
    private readonly notifier: NotifierService;

    public personsList: string[] = [];

    personModel = new Person();
    submitted = false;

    constructor (private personService: PersonService, notifierService: NotifierService) {
        this.notifier = notifierService;
    }

    ngOnInit(): void {
        this.personService.getPersons().subscribe(result => {
            this.personsList = result;
        });
    }

    onSubmit() { 
        this.submitted = true;
        this.personService.addPerson(this.personModel).subscribe(result => {
            this.notifier.notify('success', 'You are awesome! I mean it!');
        },
        () => {
            alert('Server error');
        });
    }

}