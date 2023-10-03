import { Component, OnInit, ViewChild } from "@angular/core";
import { PersonService } from "../services/persons.service";
import { Person } from "../shared/models/person.model";
import { FormsModule } from "@angular/forms";
import { NotifierService } from 'angular-notifier';
import { ToastrService } from "ngx-toastr";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
    selector:'persons-component',
    templateUrl: './persons.component.html'
})
export class PersonsComponent implements OnInit {
    private notifier: NotifierService;

    public personsList: Person[] = [];
    dataSource = new MatTableDataSource<Person>();
    @ViewChild(MatSort) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator; 
    displayedColumns: string[] = ['id', 'firstName', 'lastName'];

    personModel = new Person();
    submitted = false;
    resLength = 0;

    constructor (private personService: PersonService, notifierService: NotifierService,
        private toastr: ToastrService) {
        this.notifier = notifierService;
    }

    ngOnInit(): void {
        this.refreshPersons();
    }

    ngAfterViewInit() {
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
      }

    onSubmit() { 
        this.submitted = true;
        this.personService.addPerson(this.personModel).subscribe(result => {
            this.toastr.success('Cool', 'It worked');
            this.personModel = new Person();
            this.refreshPersons();
        },
        () => {
            alert('Server error');
        });
    }

    refreshPersons() {
        this.personService.getPersons().subscribe(result => {
            this.personsList = result;
            this.dataSource = new MatTableDataSource<Person>(this.personsList);
            this.resLength = this.personsList.length;
        });
    }

    onChangePage(pe:PageEvent) {
        console.log(pe.pageIndex);
        console.log(pe.pageSize);
      }

}