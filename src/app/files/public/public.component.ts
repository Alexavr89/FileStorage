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
}
