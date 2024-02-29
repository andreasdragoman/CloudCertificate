import { Component, OnInit, ViewChild } from "@angular/core";
import { PersonService } from "../services/persons.service";
import { Person } from "../shared/models/person.model";
import { FormsModule } from "@angular/forms";
import { NotifierService } from 'angular-notifier';
import { ToastrService } from "ngx-toastr";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatButton } from "@angular/material/button";
import { BlobsService } from "../services/blobs.service";

@Component({
    selector:'persons-component',
    templateUrl: './persons.component.html',
    styleUrls: ['./persons.component.scss']
})
export class PersonsComponent implements OnInit {
    public personsList: Person[] = [];
    dataSource = new MatTableDataSource<Person>(this.personsList);

    @ViewChild('paginator') paginator: MatPaginator;
    @ViewChild('personsTableSort') personsTableSort = new MatSort();

    displayedColumns: string[] = ['id', 'firstName', 'lastName', 'edit', 'delete'];

    newPersonModel = new Person();
    editPersonModel = new Person();
    submitted = false;
    resLength = 0;

    createNewPersonActionStarted = false;
    updatePersonActionStarted = false;
    currentEditingPersonName = "";

    constructor (private personService: PersonService, 
        private blobsService: BlobsService,
        private toastr: ToastrService) {
    }

    ngOnInit(): void {
    }

    ngAfterViewInit() {
        this.refreshPersons();
    }

    refreshPersons() {
        this.updatePersonActionStarted = false;
        this.createNewPersonActionStarted = false;
        this.currentEditingPersonName = "";
        this.newPersonModel = new Person();
        this.editPersonModel = new Person();
        this.personService.getPersons().subscribe(result => {
            this.personsList = result;
            this.dataSource = new MatTableDataSource<Person>(this.personsList);
            this.resLength = this.personsList.length;
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.personsTableSort;
            this.toastr.success('Cool', 'Refreshed');
        });
    }

    showEditPersonForm(personDetails: Person) {
        this.updatePersonActionStarted = true;
        this.editPersonModel.id = personDetails.id;
        this.editPersonModel.firstName = personDetails.firstName;
        this.editPersonModel.lastName = personDetails.lastName;
        this.currentEditingPersonName = personDetails.firstName + " " + personDetails.lastName;
    }

    hideEditPersonForm(){
        this.updatePersonActionStarted = false;
        this.editPersonModel = new Person();
    }

    deletePerson(personId: string) {
        this.personService.deletePerson(personId).subscribe(result => {
            this.toastr.success('Cool', 'Succesfully deleted');
            this.refreshPersons();
        },
        () => {
            this.toastr.error('Server error');
        });
    }

    showCreatePersonForm(show: boolean) {
        this.createNewPersonActionStarted = show;
    }

    onSubmitCreate() { 
        this.submitted = true;
        this.personService.addPerson(this.newPersonModel).subscribe(result => {
            this.toastr.success('Cool', 'Successfully created');
            this.newPersonModel = new Person();
            this.refreshPersons();
        },
        () => {
            this.toastr.error('Server error');
        });
    }

    onSubmitUpdate() { 
        this.submitted = true;
        this.personService.updatePerson(this.editPersonModel).subscribe(result => {
            this.toastr.success('Cool', 'Successfully updated');
            this.editPersonModel = new Person();
            this.refreshPersons();
        },
        () => {
            this.toastr.error('Server error');
        });
    }
}