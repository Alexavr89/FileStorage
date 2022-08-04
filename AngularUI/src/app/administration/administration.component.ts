import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Router, CanActivate } from '@angular/router';

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
  link:any;
  totalLength:any;
  page:number=1;
  pageUser:number=1;
  totalLengthUser:any;
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
    this.httpClient.delete(this.BaseUrl + "users/" + user.userId)
    .subscribe(
      (res)=>this.getUsers(),
      (err)=> console.log(err)
    )
  }
  deleteFile(file:any){
    this.httpClient.delete(this.BaseUrl + "files/" + file.id).subscribe(
      ()=>this.getFiles(''),
      (err)=>console.log(err)
    )
  }
  shareFile(file:any){
    let popup = document.getElementById("overlay")!;
    popup.style.position ="absolute";
    popup.style.opacity =".5";
    popup.style.zIndex ="998";
    popup.style.left ="0px";
    popup.style.width="100%";
    document.getElementById("popup")!.style.display = "block";  
    this.link = this.BaseUrl + "files/download/" + file.id;
  }
  close(){
    let popup = document.getElementById("overlay")!;
    popup.style.position ="";
    popup.style.opacity ="";
    popup.style.zIndex ="";
    popup.style.left ="";
    document.getElementById("popup")!.style.display = "";
  }
  getFiles(query:any){
    this.httpClient.get(this.BaseUrl + "files/search/" + query).subscribe(
      (res)=>this.files=res,
      (err)=>console.log(err)
    )
  }
  
}
