import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css']
})
export class AdministrationComponent implements OnInit {
  constructor(private httpClient:HttpClient, private router:Router) { }
  BaseUrl = 'https://localhost:44346/';
  Users: any
  ngOnInit(): void {
  }
  getUsers(){
  this.httpClient.get(this.BaseUrl + "users")
  .subscribe(
    (res)=>{
      this.Users = res;
    },
    (err)=>console.log(err)
  )
  }
  deleteUser(user: any){
    this.httpClient.delete(this.BaseUrl + "users/" + user.id)
    .subscribe(
      (res)=>{this.getUsers() },
      (err)=> console.log(err)
    )
  }
}
