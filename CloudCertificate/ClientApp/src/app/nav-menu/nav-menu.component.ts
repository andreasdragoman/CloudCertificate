import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { UserStoreService } from '../services/user-store.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  fullname: string = "";
  isUserLoggedIn: boolean = false;

  constructor(private accountService: AccountService, private userStore: UserStoreService) {
  }

  ngOnInit(): void {
    this.userStore.getFullNameFromStore()
      .subscribe(val => {
        let fullNameFromToken = this.accountService.getFullNameFromToken();
        this.fullname = val || fullNameFromToken;
      });

    this.userStore.getIsUserLoggedInFromStore()
      .subscribe(val => {
        let isUserLoggedInFromToken = this.accountService.isLoggedIn();
        this.isUserLoggedIn = val || isUserLoggedInFromToken;
      });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this.accountService.logout();
  }
}
