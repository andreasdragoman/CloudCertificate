import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import ValidateForm from "src/app/helpers/validateForm";
import { RegisterModel } from "src/app/models/authentication/register.model";
import { AccountService } from "src/app/services/account.service";

@Component({
    selector:'register-component',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

    type: string = "password";
    isText: boolean = false;
    eyeIcon: string = "fa-eye-slash";
    registerForm!: FormGroup;

    constructor(private fb: FormBuilder
        , private accountService: AccountService
        , private toastr: ToastrService
        , private router: Router) { 

    }
    
    ngOnInit(): void {
        this.registerForm = this.fb.group({
            username: ['', Validators.required],
            email: ['', Validators.required],
            name: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

    hideShowPass() {
        this.isText = !this.isText;
        this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
        this.isText ? this.type = "text" : this.type = "password";
    }

    onSubmit() {
        if (this.registerForm.valid) {
            // send obj to database
            var registerModel = new RegisterModel();
            registerModel.name = this.registerForm.controls['name'].value;
            registerModel.username = this.registerForm.controls['username'].value;
            registerModel.email = this.registerForm.controls['email'].value;
            registerModel.password = this.registerForm.controls['password'].value;
            registerModel.confirmPassword = registerModel.password;

            this.accountService.register(registerModel).subscribe({
                next: (res) => {
                    this.registerForm.reset();
                    this.toastr.success('Success register');
                    this.router.navigate(['/']);
                },
                error:(err) => {
                    this.toastr.error('Error on register');
                }
            });
        }
        else {
            // throw error using toaster
            ValidateForm.validateAllFormFields(this.registerForm);
        }
    }
}