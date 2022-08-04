import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import {Router} from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})

export class RegistrationComponent implements OnInit {
  constructor(private formBuilder: FormBuilder, private httpClient: HttpClient, private router:Router) { }
  passwordEqualityValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const Password = control.get('Password');
    const PasswordConfirm = control.get('PasswordConfirm'); 
    return Password?.value === PasswordConfirm?.value ? null : { isNotEqual: true };
  }
  BaseUrl = 'https://localhost:44346/';
  registerForm = this.formBuilder.group({
    login: ['', [Validators.required]],
    Password: ['', [Validators.required]],
    PasswordConfirm:['', Validators.required],
    email:['', Validators.email]
  }, { validator: this.passwordEqualityValidator });

  ngOnInit(): void {
  }

  onSubmit(){
    if (this.registerForm.valid){
      let a = this.registerForm.value;
      this.httpClient.post(this.BaseUrl+'auth/register', this.registerForm.value).subscribe(
        ()=> this.router.navigate(['/']),
        (err)=> console.log(err)
      );
    }
  }

  get email(){
    return this.registerForm.get('email');
  }
  get Password(){
    return this.registerForm.get('Password');
  }
  get PasswordConfirm(){
    return this.registerForm.get('PasswordConfirm');
  }
  get login(){
    return this.registerForm.get('login');
  }
}
