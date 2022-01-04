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
  userId:any;
  BaseUrl = 'https://localhost:44346/';

  ngOnInit(): void {
    this.getFiles();
  }
  getFiles(){
    this.httpClient.get(this.BaseUrl + "files/" + this.userId).subscribe(
      (res)=>this.files=res,
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
    formData.append('uploadedFile', uploadedFile, uploadedFile.name);
    this.httpClient.post(this.BaseUrl + "files/upload", formData, httpOptions).subscribe(
      ()=>this.getFiles(),
      (err)=>console.log(err)
    )
  }
}
