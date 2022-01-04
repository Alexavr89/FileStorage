import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css']
})
export class AdministrationComponent implements OnInit {
  constructor(private httpClient:HttpClient, private router:Router) { }
  BaseUrl = 'https://localhost:44346/';
  Users: any;
  files: any;
  ngOnInit(): void {
    this.getUsers();
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
  deleteFile(file:any){
    this.httpClient.delete(this.BaseUrl + "files/" + file.id).subscribe(
      ()=>this.getFiles(null),
      (err)=>console.log(err)
    )
  }
  shareFile(file:any){
    this.httpClient.get(this.BaseUrl + "files/download/" + file.id, {
      observe: 'response',
      responseType: 'blob'
  }).subscribe(
      (data)=> {
        switch (data.type) {
          case HttpEventType.Response:
            const downloadedFile = new Blob([data.body!], { type: data.body!.type });
            document.getElementById(file.name)!.innerHTML = URL.createObjectURL(downloadedFile);
            break;
        }
      },
      (err)=>console.log(err)
    )
  }
  getFiles(query:any){
    this.httpClient.get(this.BaseUrl + "files/search/" + query).subscribe(
      (res)=>this.files=res,
      (err)=>console.log(err)
    )
  }
  
}
