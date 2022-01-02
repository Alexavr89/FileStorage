import { Component } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';  
import { map } from 'rxjs/operators';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent{
  constructor(private formBuilder: FormBuilder, private httpClient: HttpClient, private router: Router) { 
  }
  title = 'AngularUI';
  BaseUrl = 'https://localhost:44346/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  loginForm = this.formBuilder.group({
    email: ['',[Validators.required]],
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
      return this.httpClient.post(this.BaseUrl+'auth/logon', this.loginForm.value)
      .pipe(
        map((response: any) => {
          const user = response;
          if (user) {
            localStorage.setItem('token', user.token);
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
            console.log(this.decodedToken);
          }
        })
      ).subscribe(() => {
        this.router.navigate(['/files']),
        document.getElementById("myForm")!.style.display = ''},
        (err)=>document.getElementById("error")!.innerHTML="Wrong login credentials please try again"
      )}
  loggedIn() {
    const token = localStorage.getItem('token')!;
    return !this.jwtHelper.isTokenExpired(token);
  }
  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/']);
  }
  get email(){
    return this.loginForm.get('email');
  }
  get password(){
    return this.loginForm.get('password');
  }
}
