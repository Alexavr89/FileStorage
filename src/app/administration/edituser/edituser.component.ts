import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';

@Component({
  selector: 'app-edituser',
  templateUrl: './edituser.component.html',
  styleUrls: ['./edituser.component.css']
})
export class EdituserComponent implements OnInit {
  constructor(private httpClient: HttpClient, private route: ActivatedRoute, 
    private formBuilder: FormBuilder, private router: Router) {
   }
  BaseUrl = 'https://localhost:44346/';
  userId : any;
  roles: any;
  user: any;
  editForm=this.formBuilder.group({
    userName: ['', Validators.required],
    email: ['', Validators.email],
    role:''
  });
  ngOnInit(): void {
    this.getUser();
    this.getRoles();
  }
  getUser(){
    this.route.queryParams
    .subscribe(params => {
      this.userId = params['userId']
    });
    this.httpClient.get(this.BaseUrl + "users/"+this.userId).subscribe(
      (res)=>this.user = res,
      (err)=>console.log(err)
    );
  }
  getRoles(){
    this.httpClient.get(this.BaseUrl + "roles").subscribe(
      (res)=> this.roles = res,
      (err)=> console.log(err)
    );
  }
   updateUser(){
     this.httpClient.put(this.BaseUrl + "users/" + this.userId + "/" + this.editForm.controls['role'].value, this.editForm.value).subscribe(
      (res)=> this.router.navigate(['/administration']),
      (err)=> console.log(err));
   }
   get email(){
    return this.editForm.get('email');
  }
  get userName(){
    return this.editForm.get('userName');
  }
}
