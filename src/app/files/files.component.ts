import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpEventType, HttpHeaders } from '@angular/common/http';
import { FormBuilder} from '@angular/forms';

@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {
  constructor(private httpClient: HttpClient, private formBuilder: FormBuilder) { }
  files:any;
  user:any;
  userId: any;
  link:any;
  totalLength:any;
  page:number=1;
  BaseUrl = 'https://localhost:44346/';

  ngOnInit(): void {
    this.getFiles();
  }
  getFiles(){
    this.userId = localStorage.getItem('userId');
    this.httpClient.get(this.BaseUrl + "files/" + this.userId).subscribe(
      (res)=>{this.files=res, this.totalLength=res.toLocaleString.length},
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
  public uploadFile = (files:any) => {
    if (files.length === 0) {
      return;
    }
    const httpOptions = {
      headers: new HttpHeaders({
      'accept': 'application/json'
    })
    }
    let uploadedFile = <File>files[0];
    const formData = new FormData();
    this.user = localStorage.getItem('user');
    formData.append('uploadedFile', uploadedFile, uploadedFile.name);
    this.httpClient.post(this.BaseUrl + "files/upload/" + this.user, formData, httpOptions).subscribe(
      ()=>this.getFiles(),
      (err)=>console.log(err)
    )
  }
}
