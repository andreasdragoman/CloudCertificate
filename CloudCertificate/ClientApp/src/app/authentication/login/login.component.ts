import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import ValidateForm from "src/app/helpers/validateForm";
import { LoginModel } from "src/app/models/authentication/login.model";
import { AccountService } from "src/app/services/account.service";

@Component({
    selector:'login-component',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

    type: string = "password";
    isText: boolean = false;
    eyeIcon: string = "fa-eye-slash";
    loginForm!: FormGroup;

    constructor(private fb: FormBuilder
        , private accountService: AccountService
        , private toastr: ToastrService
        , private router: Router) { 

    }
    
    ngOnInit(): void {
        this.loginForm = this.fb.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

    hideShowPass() {
        this.isText = !this.isText;
        this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
        this.isText ? this.type = "text" : this.type = "password";
    }

    onSubmit() {
        if (this.loginForm.valid) {
            var loginModel = new LoginModel();
            loginModel.username = this.loginForm.controls['username'].value;
            loginModel.password = this.loginForm.controls['password'].value;
            loginModel.rememberMe = false;

            this.accountService.login(loginModel).subscribe({
                next: (res) => {
                    this.loginForm.reset();
                    this.toastr.success('Success logging');
                    this.router.navigate(['/']);
                },
                error:(err) => {
                    this.toastr.error('Error on logging');
                }
            });
            
        } else {
            ValidateForm.validateAllFormFields(this.loginForm);
        }
    }
}