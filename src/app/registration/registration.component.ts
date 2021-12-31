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
    const password = control.get('password');
    const confirmpassword = control.get('confirmpassword'); 
    return password?.value === confirmpassword?.value ? null : { isNotEqual: true };
  }
  BaseUrl = 'https://localhost:44346/';
  registerForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
    confirmpassword:['', Validators.required],
    year:'',
  }, { validator: this.passwordEqualityValidator });

  ngOnInit(): void {
  }

  onSubmit(){
    if (this.registerForm.valid){
      this.httpClient.post(this.BaseUrl+'auth/register', this.registerForm.value).subscribe(
        ()=> this.router.navigate(['/']),
        (err)=> console.log(err)
      );
    }
  }
  /** A hero's name can't match the hero's alter ego */

  get email(){
    return this.registerForm.get('email');
  }
  get password(){
    return this.registerForm.get('password');
  }
  get confirmpassword(){
    return this.registerForm.get('confirmpassword');
  }
}
