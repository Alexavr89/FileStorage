import { style } from '@angular/animations';
import { Component } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent{
  constructor(private formBuilder: FormBuilder, private httpClient: HttpClient) { 
  }
  title = 'AngularUI';
  BaseUrl = 'https://localhost:44346/';
  loginForm = this.formBuilder.group({
    email: ['',[Validators.required, Validators.email]],
    password: ['',Validators.required]
  });
  openForm(){
    let formDisplay = document.getElementById("myForm");
    if (formDisplay!.style.display == '') {
      formDisplay!.style.display = "block";
    }
    else {
      formDisplay!.style.display = '';
    }
  }
  onSubmit(){
    if (this.loginForm.valid){
      this.httpClient.post(this.BaseUrl+'auth/logon', this.loginForm.value).subscribe(
        (res)=> res,
        (err)=> console.log(err)
      );
    }
  }
  get email(){
    return this.loginForm.get('email');
  }
  get password(){
    return this.loginForm.get('password');
  }
}
