import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-public',
  templateUrl: './public.component.html',
  styleUrls: ['./public.component.css']
})
export class PublicComponent implements OnInit {

  constructor(private httpClient: HttpClient, private formBuilder: FormBuilder) { }
  BaseUrl = 'https://localhost:44346/';
  files:any;
  userId:any;
  link:any;
  totalLength:any;
  page:number=1;
  ngOnInit(): void {
    this.getFiles()
  }
  getFiles(){
    this.userId = localStorage.getItem('userId');
    this.httpClient.get(this.BaseUrl + "files/public/" + this.userId).subscribe(
      (res)=>this.files=res,
      (err)=>console.log(err) 
    )
  }
  setPrivate(file:any){
    const IsPrivate = false;
    this.httpClient.post(this.BaseUrl + "files/setprivate/" + file.id, IsPrivate).subscribe(
      (res)=>this.getFiles(),
      (err)=>console.log(err)
    )
  }
  deleteFile(file:any){
    this.httpClient.delete(this.BaseUrl + "files/" + file.id).subscribe(
      ()=>this.getFiles(),
      (err)=>console.log(err)
    )
  }
  shareFile(file:any){
    let popup = document.getElementById("overlay")!;
    popup.style.position ="absolute";
    popup.style.opacity =".5";
    popup.style.zIndex ="998";
    popup.style.left ="170px";
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
}
