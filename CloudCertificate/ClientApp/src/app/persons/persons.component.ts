import { Component, OnInit } from "@angular/core";
import { PersonService } from "../services/persons.service";
import { Person } from "../shared/models/person.model";
import { FormsModule } from "@angular/forms";
import { NotifierService } from 'angular-notifier';
import { ToastrService } from "ngx-toastr";

@Component({
    selector:'persons-component',
    templateUrl: './persons.component.html'
})
export class PersonsComponent implements OnInit {
    private notifier: NotifierService;

    public personsList: Person[] = [];

    personModel = new Person();
    submitted = false;

    constructor (private personService: PersonService, notifierService: NotifierService,
        private toastr: ToastrService) {
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
            this.toastr.success('Cool', 'It worked');
        },
        () => {
            alert('Server error');
        });
    }

}